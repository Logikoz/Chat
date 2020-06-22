using Chat.Dependencies;
using Chat.Dependencies.Models;

using Microsoft.AspNetCore.SignalR.Client;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace Chat.Client.Mobile.ViewModels
{
	internal class ChatViewModel : ViewModelBase
	{
		private HubConnection hubConnection;

		private MessageModel _user;

		public MessageModel User
		{
			get => _user;
			set => Set(ref _user, value);
		}

		public ObservableCollection<MessageModel> Messages { get; private set; } = new ObservableCollection<MessageModel>();

		public ICommand ConnectionCommand { get; private set; }
		public ICommand DesconnectionCommand { get; private set; }
		public ICommand SendMessageCommand { get; private set; }

		public ChatViewModel() => Init();

		private void Init()
		{
			User = new MessageModel();

			hubConnection = new HubConnectionBuilder().WithUrl("http://logikoz.ddns.net:5000/ChatHub").Build();

			hubConnection.On<string, string>("ReceiveMessage", (user, message) => SendMessageChat(new MessageModel { Name = user, Message = message }));

			hubConnection.On<string>("JoinChat", (user) => SendMessageChat(new MessageModel { Name = user, Message = "Entrou no chat!" }));

			hubConnection.On<string>("LeaveChat", (user) => SendMessageChat(new MessageModel { Name = user, Message = "Saiu do chat!" }));

			void SendMessageChat(MessageModel message) => Messages.Add(message);

			//commands
			ConnectionCommand = new Command(async () => await ConnectionAsync());
			DesconnectionCommand = new Command(async () => await DesconnectionAsync());
			SendMessageCommand = new Command(async () => await SendMessageAsync());
		}

		private async Task ConnectionAsync()
		{
			await hubConnection.StartAsync();
			await hubConnection.InvokeAsync("JoinChat", User.Name);
		}

		private async Task DesconnectionAsync()
		{
			await hubConnection.InvokeAsync("LeaveChat", User.Name);
			await hubConnection.StopAsync();
		}

		private async Task SendMessageAsync()
		{
			await hubConnection.InvokeAsync("SendMessage", User.Name, User.Message);
			User.Message = null;
		}
	}
}