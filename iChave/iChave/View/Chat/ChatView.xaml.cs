using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace iChave.View.Chat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatView : ContentPage
	{
		public ChatView ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, true);
			On<iOS>().SetUseSafeArea(true);
		}
	}
}