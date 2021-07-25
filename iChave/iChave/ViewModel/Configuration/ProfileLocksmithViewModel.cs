using FluentValidation.Results;
using iChave.Model;
using iChave.ModelDTO;
using iChave.ModelRealm;
using iChave.Validator;
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
    public class ProfileLocksmithViewModel : ViewModelBase
    {
        protected ProfileLocksmithViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
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

        private string _stateRegistration;
        public string StateRegistration
        {
            get => _stateRegistration;
            set => SetProperty(ref _stateRegistration, value);
        }
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            GetLocksmith();
            HasInitialized = true;
        }

        public async void GetLocksmith()
        {
            try
            {
                IsBusy = true;
                TokenFirebaseRealm token = Realm.GetInstance().All<TokenFirebaseRealm>().First();
                LocksmithDTO apiRetorno = await Util.GetApi().GetLocksmithByEmail(token.Email);
                CompanyName = apiRetorno.company_name;
                Email = apiRetorno.email;
                Phone = apiRetorno.phone;
                Cnpj = apiRetorno.cnpj;
                StateRegistration = apiRetorno.state_registration;
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

        private DelegateCommand updateCommand;
        public DelegateCommand UpdateCommand => updateCommand ??= new DelegateCommand(async () => await UpdateAsync(), () => !IsBusy);

        private async Task UpdateAsync()
        {
            try
            {
                IsBusy = true;
                SmallLocksmithVal _locksmith = new SmallLocksmithVal()
                {
                    company_name = CompanyName,
                    cnpj = Cnpj,
                    phone = Phone,
                    email = Email,
                    state_registration = StateRegistration
                };
                ValidationResult resultValidation = new SmallLocksmithValidator().Validate(_locksmith);
                if (resultValidation.IsValid)
                {
                    LocksmithDTO _locksmithDto = new LocksmithDTO()
                    {
                        company_name = CompanyName,
                        cnpj = Cnpj.Replace(".", "").Replace("/", "").Replace("-", ""),
                        phone = Phone.Replace("(", "").Replace(")", "").Replace("-", ""),
                        email = Email,
                        state_registration = StateRegistration
                    };
                    Msg apiRetorno = await Util.GetApi().UpdateLocksmith(_locksmithDto);
                    if (apiRetorno.msg == Msg.SUCCESS)
                    {
                        await PageDialogService.DisplayAlertAsync("Atualizado", "Chaveiro Atualizado!", "OK");
                        await NavigationService.GoBackAsync();
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("Erro", apiRetorno.msg, "OK");
                    }
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Requerer campos:", string.Join("\n", resultValidation.Errors), "OK");
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
