// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var connection = new signalR.HubConnectionBuilder().withUrl("/learningHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

 connection.start();

connection.on("ReceiveMessage", (message) => {
    $('#signalr-message-panel').prepend($('<div />').text(message));
});

$('#btn-broadcast').click(function () {
    var message = $('#broadcast').val();
    connection.invoke("BroadCastMessage", message).catch(err =>
        console.error(err.toString())
    );
});

async function start() {
    try {

    } catch (err) {
        console.log(err);
        setTimeout(() => start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

