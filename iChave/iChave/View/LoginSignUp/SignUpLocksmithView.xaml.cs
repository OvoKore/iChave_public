using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace iChave.View.LoginSignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpLocksmithView : ContentPage
    {
        public SignUpLocksmithView()
        {
            InitializeComponent();
            Title = "Inscreva-se";
            NavigationPage.SetHasNavigationBar(this, true);
            On<iOS>().SetUseSafeArea(true);
        }
    }
}