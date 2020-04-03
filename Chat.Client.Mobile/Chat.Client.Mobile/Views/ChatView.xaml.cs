using Chat.Client.Mobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chat.Client.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatView : ContentPage
    {
        public ChatView()
        {
            InitializeComponent();
            BindingContext = new ChatViewModel();
        }
    }
}