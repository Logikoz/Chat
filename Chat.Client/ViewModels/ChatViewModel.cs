using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Microsoft.AspNetCore.SignalR.Client;

using System.Threading.Tasks;
using System.Windows.Controls;

namespace Chat.Client.ViewModels
{
    internal class ChatViewModel : ViewModelBase
    {
        private HubConnection hubConnection;
        private string _message;
        private string _userName;
        private readonly RichTextBox _chat;

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }

        public string UserName
        {
            get => _userName;
            set => Set(ref _userName, value);
        }

        public RelayCommand ConnectCommand { get; set; }
        public RelayCommand DesconnectCommand { get; set; }
        public RelayCommand SendMessageCommand { get; set; }

        public ChatViewModel(RichTextBox chat)
        {
            Init();

            _chat = chat;
        }

        private void Init()
        {
            hubConnection = new HubConnectionBuilder().WithUrl("http://192.168.1.10:5000/ChatHub").Build();

            hubConnection.On<string, string>("ReceiveMessage", (user, message) => SendMessageChat($"{user}: {message}"));

            hubConnection.On<string>("JoinChat", (user) => SendMessageChat($"{user} entrou no chat!"));

            hubConnection.On<string>("LeaveChat", (user) => SendMessageChat($"{user} saiu do chat!"));

            void SendMessageChat(string msg)
            {
                _chat.AppendText(msg + "\n");
                _chat.ScrollToEnd();
            }

            //commands
            ConnectCommand = new RelayCommand(async () => await ConnectAsync());
            DesconnectCommand = new RelayCommand(async () => await DesconnectAsync());
            SendMessageCommand = new RelayCommand(async () => await SendMessageAsync());
        }

        private async Task SendMessageAsync()
        {
            await hubConnection.InvokeAsync("SendMessage", UserName, Message);
        }

        private async Task DesconnectAsync()
        {
            await hubConnection.InvokeAsync("LeaveChat", UserName);
            await hubConnection.StopAsync();
        }

        private async Task ConnectAsync()
        {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("JoinChat", UserName);
        }
    }
}