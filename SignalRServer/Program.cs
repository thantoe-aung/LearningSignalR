using MessagePack;
using Microsoft.AspNetCore.Http.Connections;
using SignalRServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Add SignalR
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.KeepAliveInterval = TimeSpan.FromSeconds(10);
    hubOptions.MaximumReceiveMessageSize = 65536;
    hubOptions.HandshakeTimeout = TimeSpan.FromSeconds(15);
    hubOptions.MaximumParallelInvocationsPerClient = 10;
    hubOptions.EnableDetailedErrors = false;
    hubOptions.StreamBufferCapacity = 1024;

    if (hubOptions?.SupportedProtocols is not null)
    {
        foreach (var protocol in hubOptions.SupportedProtocols)
        {
            Console.WriteLine(protocol.ToString());
        }
    }
}).AddJsonProtocol(opt =>
{
    opt.PayloadSerializerOptions.PropertyNamingPolicy = null;

})
.AddMessagePackProtocol(options =>
{
    options.SerializerOptions = MessagePack.MessagePackSerializerOptions.Standard
    .WithSecurity(MessagePackSecurity.UntrustedData)
    .WithCompression(MessagePackCompression.Lz4Block)
    .WithOldSpec()
    .WithOmitAssemblyVersion(true);

});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<LearningHub>("/learningHub", options =>
{
    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
    options.CloseOnAuthenticationExpiration = true;
    options.WebSockets.CloseTimeout =   TimeSpan.FromSeconds(15);
    options.TransportSendTimeout = TimeSpan.FromSeconds(10);
});

app.UseBlazorFrameworkFiles();

app.Run();
