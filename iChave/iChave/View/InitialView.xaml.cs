using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace iChave.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InitialView : ContentPage
    {
        public InitialView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}