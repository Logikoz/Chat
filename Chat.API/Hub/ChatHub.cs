using Microsoft.AspNetCore.SignalR;

using System.Threading.Tasks;

using SignalR = Microsoft.AspNetCore.SignalR;

namespace Chat.API.Hub
{
    public class ChatHub : SignalR::Hub, IChatHub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task JoinChat(string user)
        {
            await Clients.All.SendAsync("JoinChat", user);
        }

        public async Task LeaveChat(string user)
        {
            await Clients.All.SendAsync("LeaveChat", user);
        }
    }
}