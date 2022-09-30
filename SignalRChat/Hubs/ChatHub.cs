using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs.Interface;
using SignalRChat.Models;
using System.Security.Claims;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private const string secretGroup = "Secret Group";
        private const string KEY = "KEY";

        private UserManager<IdentityUser> _userManager;
        private IHttpContextAccessor _httpContextAccessor;

        public ChatHub(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HubMethodName("SendMessageToGroup")]
        public async Task SendMessageToGroupAsync(string user, string message)
            => await Clients.Group(secretGroup).NotifyGroup(user, message);

        [HubMethodName("SendMessageToAllClient")]
        public async Task SendMessageToAllClientAsync(Info info)
        {
            Context.Items.Add(KEY, "This is value from Context.Items");
            await Clients.All.ReceiveMessage(new Info() { User = _userManager.GetUserName(_httpContextAccessor.HttpContext.User).ToString(), Message = info.Message, Addition = Context.Items[KEY]?.ToString() ?? String.Empty });
        }

        [HubMethodName("JoinSecretGroup")]
        public async Task JoinSecretGroupAsync()
            => await Groups.AddToGroupAsync(Context.ConnectionId, secretGroup);

        [HubMethodName("SendPrivateMessage")]
        public async Task SendPrivateMessageAsync(string user, string message)
        {
            await Clients.User(user).ReceivePrivateMessage(new Info() { User = user, Message = message });
        }

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
