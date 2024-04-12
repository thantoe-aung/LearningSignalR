// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var connection = new signalR.HubConnectionBuilder().withUrl("/learningHub", {
   
}) .configureLogging(signalR.LogLevel.Information)
    .build();

 connection.start();

connection.on("ReceiveMessage", (message) => {
    $('#signalr-message-panel').prepend($('<div />').text(message));
});

$('#btn-broadcast').click(function () {
    //var message = $('#broadcast').val();
    //connection.invoke("BroadCastMessage", message).catch(err =>
    //    console.error(err.toString())
    //);

    var message = $('#broadcast').val();
    if (message.includes(';')) {
        var messages = message.split(';');

        var subject = new signalR.Subject();
        connection.send("BroadCastStream", subject).catch(err =>
            console.error(err.toString())
        );

        for (var i = 0; i < messages.length; i++) {
            subject.next(messages[i]);
        }

        subject.complete();

    } else {
        connection.invoke("BroadCastMessage", message).catch(err =>
            console.error(err.toString())
        );
    }
});


$('#btn-trigger-stream').click(function () {
    var numberOfJobs = parseInt($('#number-of-jobs').val(), 10);

    connection.stream("TriggerStream", numberOfJobs).subscribe({
        next: (message) => $('#signalr-message-panel').prepend($('<div />').text(message))
    });

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

