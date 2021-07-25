using iChave.Model;
using iChave.ModelDTO;
using iChave.ModelRealm;
using iChave.ViewModel.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace iChave.ViewModel.Configuration
{
    public class ConfigViewModel : ViewModelTabbedBase
    {
        protected ConfigViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        private DelegateCommand aboutCommand;
        public DelegateCommand AboutCommand => aboutCommand ??= new DelegateCommand(async () => await Navigation("AboutView"), () => !IsBusy);

        private DelegateCommand addressCommand;
        public DelegateCommand AddressCommand => addressCommand ??= new DelegateCommand(async () => await Navigation("AddressView"), () => !IsBusy);


        private DelegateCommand profileCommand;
        public DelegateCommand ProfileCommand => profileCommand ??= new DelegateCommand(async () => await ProfileAsync(), () => !IsBusy);

        private async Task ProfileAsync()
        {
            try
            {
                IsBusy = true;
                string Role = Realm.GetInstance().All<TokenFirebaseRealm>().First().Role;
                await Navigation(Role == "user" ? "ProfileUserView" : "ProfileLocksmithView");
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        private DelegateCommand logoutCommand;
        public DelegateCommand LogoutCommand => logoutCommand ??= new DelegateCommand(async () => await LogoutAsync(), () => !IsBusy);

        private async Task LogoutAsync()
        {
            try
            {
                bool result = await PageDialogService.DisplayAlertAsync("CONFIRMAR", "Você realmente quer deslogar?", "Sim", "Não");
                if (result)
                {
                    IsBusy = true;
                    Realm realm = Realm.GetInstance();
                    TokenFirebaseRealm token = realm.All<TokenFirebaseRealm>().FirstOrDefault();
                    Msg msg = await Util.GetApi().Logout(new LogoutDTO(token.Email, token.Role));
                    if (msg.msg == Msg.SUCCESS)
                    {
                        new ReloadRealm().Logout();
                        await NavigationService.NavigateAsync("/NavigationPage/LoginView");
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("Erro", "Erro ao deslogar", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
