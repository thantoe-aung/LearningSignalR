using Microsoft.AspNetCore.SignalR;

namespace SignalRServer.Hubs
{
    public class LearningHub : Hub<ILearningHubClient>
    {
        public async Task BroadCastMessage(string message)
        {
            await Clients.All.ReceiveMessage(message);
        }

        public async Task SendToOthers(string message)
        {
            await Clients.Others.ReceiveMessage(message);  
        }

        public async Task SendToIndividual(string connectionId,string message)
        {
            await Clients.Client(connectionId).ReceiveMessage(GetMessageToSend(message));
        }


        public async Task SendToCaller(string message)
        {
            await Clients.Caller.ReceiveMessage(GetMessageToSend(message));
        }

        public async Task SendToGroup(string groupName,string maessage)
        {
            await Clients.Group(groupName).ReceiveMessage(GetMessageToSend(groupName)); 
        }

        public async Task AddUserToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveUserFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        private string GetMessageToSend(string originalMessage)
        {
            return $"User Connection Id : {Context.ConnectionId}. Message : {originalMessage}";
        }

        public override async Task OnConnectedAsync()
        {
             await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
