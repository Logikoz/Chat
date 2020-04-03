using System.Threading.Tasks;

namespace Chat.API.Hub
{
    internal interface IChatHub
    {
        Task SendMessage(string user, string message);

        Task JoinChat(string user);

        Task LeaveChat(string user);
    }
}