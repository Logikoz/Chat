using Microsoft.AspNetCore.SignalR.Client;

using System;
using System.Threading.Tasks;

namespace Chat.Client.Site.Services
{
    public class ChatComunicationSevice
    {
        private HubConnection hubConnection;

        public string UserName { get; set; }
        public string Message { get; set; }

        public void Init()
        {
            hubConnection = new HubConnectionBuilder().WithUrl("http://logikoz.ddns.net:5000/ChatHub").Build();

            //hubConnection.On<string, string>("ReceiveMessage", (user, message) => SendMessageChat(new MessageModel { Name = user, Message = message }));

            //hubConnection.On<string>("JoinChat", (user) => SendMessageChat(new MessageModel { Name = user, Message = "Entrou no chat!" }));

            //hubConnection.On<string>("LeaveChat", (user) => SendMessageChat(new MessageModel { Name = user, Message = "Saiu do chat!" }));
        }

        private async Task ConnectionAsync()
        {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("JoinChat", UserName);
        }

        private async Task DesconnectionAsync()
        {
            await hubConnection.InvokeAsync("LeaveChat", UserName);
            await hubConnection.StopAsync();
        }

        private async Task SendMessageAsync()
        {
            await hubConnection.InvokeAsync("SendMessage", UserName, Message);
        }
    }
}