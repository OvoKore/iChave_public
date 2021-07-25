using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using iChave.Model;
using iChave.ModelDTO;
using iChave.ModelRealm;
using iChave.Service;
using iChave.ViewModel.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;

namespace iChave.ViewModel.LoginSignUp
{
    public class LoginViewModel : ViewModelBase
    {
        protected LoginViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Email = string.Empty;
            Password = string.Empty;
        }

        #region Fields
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
        #endregion

        private DelegateCommand loginCommand;
        public DelegateCommand LoginCommand => loginCommand ??= new DelegateCommand(async () => await LoginAsync(), () => !IsBusy);

        private async Task LoginAsync()
        {
            try
            {
                IsBusy = true;
                if (!(string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password)))
                {
                    if (Util.EmailValidator(Email) && Util.PasswordValidator(Password))
                    {
                        try
                        {
                            FirebaseAuthProvider authFirebase = Util.GetAuthFirebase();
                            FirebaseAuthLink auth = await authFirebase.SignInWithEmailAndPasswordAsync(Email.ToLower(), Password);
                            if (auth.User.IsEmailVerified)
                            {
                                Realm realm = Realm.GetInstance();
                                TokenFCMRealm tokenFCM = realm.All<TokenFCMRealm>().FirstOrDefault();
                                string fcm = tokenFCM != null ? tokenFCM.Token : "";

                                IRestApi API = Util.GetApi();
                                UpdateFcmDTO msg = await API.UpdateFCM(new UpdateFcmDTO(Email.ToLower(), fcm));

                                realm.Write(() =>
                                {
                                    realm.RemoveAll<TokenFirebaseRealm>();
                                    realm.Add(new TokenFirebaseRealm()
                                    {
                                        FirebaseToken = auth.FirebaseToken,
                                        RefreshToken = auth.RefreshToken,
                                        Role = msg.role,
                                        Email = Email.ToLower(),
                                        CpfCnpj = msg.cpf_cnpj
                                    });
                                });
                                await Navigation(msg.role == "user" ? "/MenuUserView" : "/MenuLocksmithView");
                            }
                            else
                            {
                                await PageDialogService.DisplayAlertAsync("Conta não validada.", "Entre no seu email e valide a sua conta.", "OK");
                            }
                        }
                        catch (FirebaseAuthException ex)
                        {
                            if (ex.Reason == AuthErrorReason.WrongPassword)
                            {
                                await PageDialogService.DisplayAlertAsync("Erro", "Email ou senha inválidos.", "OK");
                            }
                            else
                            {
                                await PageDialogService.DisplayAlertAsync(ex.ResponseData, ex.Message, "OK");
                            }
                        }
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("Erro", "Email ou senha inválidos.", "OK");
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

        private DelegateCommand forgetPasswordCommand;
        public DelegateCommand ForgetPasswordCommand => forgetPasswordCommand ??= new DelegateCommand(async () => await Navigation("ForgetPasswordView"), () => !IsBusy);

        private DelegateCommand signUpUserCommand;
        public DelegateCommand SignUpUserCommand => signUpUserCommand ??= new DelegateCommand(async () => await Navigation("SignUpUserView"), () => !IsBusy);

        private DelegateCommand signUpLocksmithCommand;
        public DelegateCommand SignUpLocksmithCommand => signUpLocksmithCommand ??= new DelegateCommand(async () => await Navigation("SignUpLocksmithView"), () => !IsBusy);
    }
}