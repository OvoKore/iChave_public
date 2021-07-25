using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace iChave.View.Show
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowLocksmithView : ContentPage
    {
        public ShowLocksmithView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}