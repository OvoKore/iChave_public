using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace iChave.View.Service
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewServiceView : ContentPage
    {
        public NewServiceView()
        {
            InitializeComponent();
            Title = "Serviços";
            NavigationPage.SetHasNavigationBar(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}