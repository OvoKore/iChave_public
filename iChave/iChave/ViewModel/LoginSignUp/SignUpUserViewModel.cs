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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChave.ViewModel.LoginSignUp
{
    public class SignUpUserViewModel : ViewModelBase
    {
        protected SignUpUserViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }
        #region Fields
        private List<string> _sexList = Util.SexList;
        public List<string> SexList
        {
            get => _sexList;
            set => SetProperty(ref _sexList, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _cpf;
        public string Cpf
        {
            get => _cpf;
            set => SetProperty(ref _cpf, value);
        }

        private string _sex = Util.SexList.First();
        public string Sex
        {
            get => _sex;
            set => SetProperty(ref _sex, value);
        }

        private DateTime _birthdate = new DateTime(2000, 01, 01);
        public DateTime Birthdate
        {
            get => _birthdate;
            set => SetProperty(ref _birthdate, value);
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
        #endregion

        private DelegateCommand registerCommand;
        public DelegateCommand RegisterCommand => registerCommand ??= new DelegateCommand(async () => await RegisterAsync(), () => !IsBusy);

        private async Task RegisterAsync()
        {
            try
            {
                IsBusy = true;
                UserVal _user = new UserVal()
                {
                    name = Name,
                    cpf = Cpf,
                    phone = Phone,
                    sex = Sex,
                    birthdate = Birthdate,
                    email = Email,
                    confirm_email = ConfirmEmail,
                    password = Password,
                    confirm_password = ConfirmPassword
                };
                ValidationResult resultValidation = new UserValidator().Validate(_user);
                if (resultValidation.IsValid)
                {
                    IRestApi API = Util.GetApi();
                    UserDTO user = new UserDTO(_user);
                    try
                    {
                        Msg usuariosRetorno = await API.CreateUser(user);
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
