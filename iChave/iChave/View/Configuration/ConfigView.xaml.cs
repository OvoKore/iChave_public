using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace iChave.View.Configuration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigView : ContentPage
    {
        public ConfigView()
        {
            InitializeComponent();
            Title = "Configuração";
            NavigationPage.SetHasNavigationBar(this, false);
            ViewModelLocator.SetAutowireViewModel(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}