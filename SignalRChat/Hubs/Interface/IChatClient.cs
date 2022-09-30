using SignalRChat.Models;

namespace SignalRChat.Hubs.Interface
{
    public interface IChatClient
    {
        Task ReceiveMessage(Info info);
        Task ReceivePrivateMessage(Info info);
        Task NotifyCurrentClient();
        Task NotifyOtherClients();
        Task NotifyGroup(string user, string message);
        Task NotifyConnectionToServer(string message);
    }
}
