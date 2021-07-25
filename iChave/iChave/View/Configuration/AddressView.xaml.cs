using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace iChave.View.Configuration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressView : ContentPage
    {
        public AddressView()
        {
            InitializeComponent();
            Title = "Endereço";
            NavigationPage.SetHasNavigationBar(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}