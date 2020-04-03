using Chat.Client.ViewModels;

using System.Windows;

namespace Chat.Client
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ChatViewModel(Chat);
        }
    }
}