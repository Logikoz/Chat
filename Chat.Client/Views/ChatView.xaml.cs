using Chat.Client.ViewModels;

using System.Windows;

namespace Chat.Client.Views
{
	public partial class ChatView : Window
	{
		public ChatView()
		{
			InitializeComponent();
			DataContext = new ChatViewModel(Chat);
			Closing += (sender, e) =>
			{
				var viewmodel = DataContext as ChatViewModel;
				viewmodel.DesconnectCommand.Execute(null);
			};
		}
	}
}