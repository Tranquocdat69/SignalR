using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs.Interface;
using SignalRChat.Models;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private const string secretGroup = "Secret Group";
        private const string KEY = "KEY";

        [HubMethodName("SendMessageToGroup")]
        public async Task SendMessageToGroupAsync(string user, string message)
            => await Clients.Group(secretGroup).NotifyGroup(user, message);

        [HubMethodName("SendMessageToAllClient")]
        public async Task SendMessageToAllClientAsync(Info info)
        {
            Context.Items.Add(KEY, "This is value from Context.Items");
            await Clients.All.ReceiveMessage(new Info() { User = info.User, Message = info.Message , Addition = Context.Items[KEY]?.ToString() ?? String.Empty });
        }

        [HubMethodName("JoinSecretGroup")]
        public async Task JoinSecretGroupAsync()
            => await Groups.AddToGroupAsync(Context.ConnectionId, secretGroup);

        [HubMethodName("SendPrivateMessage")]
        public async Task SendPrivateMessageAsync(string user, string message) 
            => await Clients.User(user).ReceivePrivateMessage(message);

        public void AbortConnection() => Context.Abort();

        public override async Task OnConnectedAsync()
        {
            await Clients.All.NotifyConnectionToServer("Connected successfuly to server");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine("Disconnected to server");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
