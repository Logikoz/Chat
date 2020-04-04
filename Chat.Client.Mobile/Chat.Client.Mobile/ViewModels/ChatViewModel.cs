using Chat.Client.Mobile.Models;

using Microsoft.AspNetCore.SignalR.Client;

using MvvmCross.Commands;
using MvvmCross.ViewModels;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chat.Client.Mobile.ViewModels
{
    internal class ChatViewModel : MvxViewModel
    {
        private HubConnection hubConnection;

        private string _userName;
        private string _message;

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ObservableCollection<MessageModel> Messages { get; private set; } = new ObservableCollection<MessageModel>();

        public ICommand ConnectionCommand { get; private set; }
        public ICommand DesconnectionCommand { get; private set; }
        public ICommand SendMessageCommand { get; private set; }

        public ChatViewModel() => Init();

        private void Init()
        {
            hubConnection = new HubConnectionBuilder().WithUrl("http://logikoz.ddns.net:5000/ChatHub").Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) => SendMessageChat(new MessageModel { Name = user, Message = message }));

            hubConnection.On<string>("JoinChat", (user) => SendMessageChat(new MessageModel { Name = user, Message = "Entrou no chat!" }));

            hubConnection.On<string>("LeaveChat", (user) => SendMessageChat(new MessageModel { Name = user, Message = "Saiu do chat!" }));

            void SendMessageChat(MessageModel message) => Messages.Add(message);

            //commands
            ConnectionCommand = new MvxCommand(async () => await ConnectionAsync());
            DesconnectionCommand = new MvxCommand(async () => await DesconnectionAsync());
            SendMessageCommand = new MvxCommand(async () => await SendMessageAsync());
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
            Message = null;
        }
    }
}