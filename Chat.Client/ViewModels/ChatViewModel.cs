using Chat.Dependencies;

using Microsoft.AspNetCore.SignalR.Client;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Chat.Client.ViewModels
{
	internal class ChatViewModel : ViewModelBase
	{
		private readonly HubConnection _hubConnection;

		private readonly RichTextBox _chat;

		private string __message;
		private string __userName;

		public string Message
		{
			get => __message;
			set => Set(ref __message, value);
		}

		public string UserName
		{
			get => __userName;
			set => Set(ref __userName, value);
		}

		public ICommand ConnectCommand { get; set; }
		public ICommand DesconnectCommand { get; set; }
		public ICommand SendMessageCommand { get; set; }

		public ChatViewModel(RichTextBox chat)
		{
			_hubConnection = new HubConnectionBuilder().WithUrl("http://192.168.1.6:5000/ChatHub").Build();

			_chat = chat;

			Init();
		}

		private void Init()
		{
			_hubConnection.On<string, string>("ReceiveMessage", (user, message) => SendMessageChat($"{user}: {message}"));

			_hubConnection.On<string>("JoinChat", (user) => SendMessageChat($"{user} entrou no chat!"));

			_hubConnection.On<string>("LeaveChat", (user) => SendMessageChat($"{user} saiu do chat!"));

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
			if (_hubConnection.State != HubConnectionState.Connected)
			{
				MessageBox.Show("Voce preicsa estar conectado para enviar mensagens");
				return;
			}

			if (string.IsNullOrEmpty(Message))
			{
				MessageBox.Show("Voce precisa informar a mensagem!");
				return;
			}

			await _hubConnection.InvokeAsync("SendMessage", UserName, Message);
			Message = string.Empty;
		}

		private async Task DesconnectAsync()
		{
			if (_hubConnection.State != HubConnectionState.Connected)
				return;

			await _hubConnection.InvokeAsync("LeaveChat", UserName);
			await _hubConnection.StopAsync();
		}

		private async Task ConnectAsync()
		{
			if (_hubConnection.State != HubConnectionState.Disconnected)
			{
				MessageBox.Show("Voce ja está conectado!");
				return;
			}

			if (string.IsNullOrEmpty(UserName))
			{
				MessageBox.Show("Voce precisa informar seu nome!");
				return;
			}

			await _hubConnection.StartAsync();
			await _hubConnection.InvokeAsync("JoinChat", UserName);
		}
	}
}