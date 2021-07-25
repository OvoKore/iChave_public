using iChave.Control;
using iChave.View.Chat;
using iChave.View.Configuration;
using iChave.View.List;
using Plugin.FirebasePushNotification;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace iChave.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuUserView : TabbedPage
    {
        public MenuUserView()
        {
            InitializeComponent();
            On<iOS>().SetUseSafeArea(true);
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            On<Android>().SetIsSmoothScrollEnabled(false);

            Children.Add(new CustomNavigationPage(new ListLocksmithView()));
            Children.Add(new CustomNavigationPage(new ChatListView()));
            Children.Add(new CustomNavigationPage(new ConfigView()));

            CrossFirebasePushNotification.Current.OnNotificationReceived += Current_OnNotificationReceived;
        }

        private void Current_OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            this.DisplaySnackBarAsync(Util.GetSnackBarOptions(e));
        }
    }
}