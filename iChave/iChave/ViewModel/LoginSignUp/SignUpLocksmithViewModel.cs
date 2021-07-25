using FluentValidation.Results;
using iChave.Model;
using iChave.ModelDTO;
using iChave.Service;
using iChave.Validator;
using iChave.ViewModel.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Refit;
using System;
using System.Threading.Tasks;

namespace iChave.ViewModel.LoginSignUp
{
    public class SignUpLocksmithViewModel : ViewModelBase
    {
        protected SignUpLocksmithViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private string _companyName;
        public string CompanyName
        {
            get => _companyName;
            set => SetProperty(ref _companyName, value);
        }

        private string _cnpj;
        public string Cnpj
        {
            get => _cnpj;
            set => SetProperty(ref _cnpj, value);
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _confirmEmail;
        public string ConfirmEmail
        {
            get => _confirmEmail;
            set => SetProperty(ref _confirmEmail, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private string _stateRegistration;
        public string StateRegistration
        {
            get => _stateRegistration;
            set => SetProperty(ref _stateRegistration, value);
        }
        #endregion

        private DelegateCommand registerCommand;
        public DelegateCommand RegisterCommand => registerCommand ??= new DelegateCommand(async () => await RegisterAsync(), () => !IsBusy);

        private async Task RegisterAsync()
        {
            try
            {
                IsBusy = true;
                LocksmithVal _locksmith = new LocksmithVal()
                {
                    company_name = CompanyName,
                    password = Password,
                    confirm_password = ConfirmPassword,
                    phone = Phone,
                    email = Email,
                    confirm_email = ConfirmEmail,
                    cnpj = Cnpj,
                    state_registration = StateRegistration
                };
                ValidationResult resultValidation = new LocksmithValidator().Validate(_locksmith);
                if (resultValidation.IsValid)
                {
                    IRestApi API = Util.GetApi();
                    LocksmithDTO locksmith = new LocksmithDTO(_locksmith);
                    try
                    {
                        Msg usuariosRetorno = await API.CreateLocksmith(locksmith);
                        if (usuariosRetorno.msg == Msg.SUCCESS)
                        {
                            await PageDialogService.DisplayAlertAsync(string.Empty, "Registro concluído.\nEntre no email para validar a sua conta.", "OK");
                            await NavigationService.GoBackAsync();
                        }
                    }
                    catch (ApiException ex)
                    {
                        await PageDialogService.DisplayAlertAsync(ex.StatusCode.ToString(), ex.GetContentAsAsync<Msg>().Result.msg, "OK");
                    }
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Campos requeridos:", string.Join("\n", resultValidation.Errors), "OK");
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
