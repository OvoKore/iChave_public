using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace iChave.View.List
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListLocksmithView : ContentPage
    {
        public ListLocksmithView()
        {
            InitializeComponent();
            Title = "Chaveiros";
            NavigationPage.SetHasNavigationBar(this, false);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}