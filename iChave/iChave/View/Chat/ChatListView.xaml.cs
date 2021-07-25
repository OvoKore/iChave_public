using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace iChave.View.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatListView : ContentPage
    {
        public ChatListView()
        {
            InitializeComponent();
            Title = "Chats";
            NavigationPage.SetHasNavigationBar(this, false);
            ViewModelLocator.SetAutowireViewModel(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}