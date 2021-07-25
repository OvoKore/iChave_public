using iChave.ModelDTO;
using iChave.ModelRealm;
using iChave.View;
using iChave.View.Chat;
using iChave.View.Configuration;
using iChave.View.List;
using iChave.View.LoginSignUp;
using iChave.View.Service;
using iChave.View.Show;
using iChave.ViewModel;
using iChave.ViewModel.Base;
using iChave.ViewModel.Chat;
using iChave.ViewModel.Configuration;
using iChave.ViewModel.List;
using iChave.ViewModel.LoginSignUp;
using iChave.ViewModel.Service;
using iChave.ViewModel.Show;
using Plugin.FirebasePushNotification;
using Prism.DryIoc;
using Prism.Ioc;
using Realms;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace iChave
{
    public partial class App : PrismApplication
    {
        public App()
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            VersionTracking.Track();

            CrossFirebasePushNotification.Current.OnTokenRefresh += Current_OnTokenRefresh;

            await NavigationService.NavigateAsync("/InitialView");
        }

        private void Current_OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Token: {e.Token}");
            Realm realm = Realm.GetInstance();
            realm.Write(() =>
            {
                realm.RemoveAll<TokenFCMRealm>();
                realm.Add(new TokenFCMRealm(e.Token));
            });
            TokenFirebaseRealm token = realm.All<TokenFirebaseRealm>().FirstOrDefault();
            if (token != null)
            {
                Util.GetApi().UpdateFCM(new UpdateFcmDTO(token.Email, e.Token));
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();

            containerRegistry.RegisterForNavigation<InitialView, InitialViewModel>();

            containerRegistry.RegisterForNavigation<MenuUserView, ViewModelBase>();
            containerRegistry.RegisterForNavigation<MenuLocksmithView, ViewModelBase>();

            containerRegistry.RegisterForNavigation<LoginView, LoginViewModel>();
            containerRegistry.RegisterForNavigation<SignUpUserView, SignUpUserViewModel>();
            containerRegistry.RegisterForNavigation<SignUpLocksmithView, SignUpLocksmithViewModel>();
            containerRegistry.RegisterForNavigation<ForgetPasswordView, ForgetPasswordViewModel>();

            containerRegistry.RegisterForNavigation<ListLocksmithView, ListLocksmithViewModel>();

            containerRegistry.RegisterForNavigation<ConfigView, ConfigViewModel>();
            containerRegistry.RegisterForNavigation<AboutView, AboutViewModel>();
            containerRegistry.RegisterForNavigation<PrivacyView, ViewModelBase>();
            containerRegistry.RegisterForNavigation<TermsUseView, ViewModelBase>();

            containerRegistry.RegisterForNavigation<AddressView, AddressViewModel>();
            containerRegistry.RegisterForNavigation<ProfileUserView, ProfileUserViewModel>();
            containerRegistry.RegisterForNavigation<ProfileLocksmithView, ProfileLocksmithViewModel>();

            containerRegistry.RegisterForNavigation<ChatListView, ChatListViewModel>();
            containerRegistry.RegisterForNavigation<ChatView, ChatViewModel>();

            containerRegistry.RegisterForNavigation<ShowLocksmithView, ShowLocksmithViewModel>();
            containerRegistry.RegisterForNavigation<ShowUserView, ShowUserViewModel>();

            containerRegistry.RegisterForNavigation<ServiceListLocksmithView, ServiceListLocksmithViewModel>();
            containerRegistry.RegisterForNavigation<NewServiceView, NewServiceViewModel>();
        }
    }
}
