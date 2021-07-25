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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iChave.ViewModel.Configuration
{
    class ProfileUserViewModel : ViewModelBase
    {
        protected ProfileUserViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private List<string> _sexList = Util.SexList;
        public List<string> SexList
        {
            get => _sexList;
            set => SetProperty(ref _sexList, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        private string _cpf;
        public string Cpf
        {
            get => _cpf;
            set => SetProperty(ref _cpf, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _sex;
        public string Sex
        {
            get => _sex;
            set => SetProperty(ref _sex, value);
        }

        private DateTime _birthdate;
        public DateTime Birthdate
        {
            get => _birthdate;
            set => SetProperty(ref _birthdate, value);
        }
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            GetUser();
            HasInitialized = true;
        }

        public async void GetUser()
        {
            try
            {
                IsBusy = true;
                TokenFirebaseRealm token = Realm.GetInstance().All<TokenFirebaseRealm>().First();
                UserDTO apiRetorno = await Util.GetApi().GetUser(token.Email);
                Name = apiRetorno.name;
                Email = apiRetorno.email;
                Phone = apiRetorno.phone;
                Cpf = apiRetorno.cpf;
                Sex = SexList.First(x => x == apiRetorno.sex);
                Birthdate = apiRetorno.birthdate;
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
                SmallUserVal _user = new SmallUserVal()
                {
                    name = Name,
                    cpf = Cpf,
                    phone = Phone,
                    email = Email,
                    sex = Sex,
                    birthdate = Birthdate
                };
                ValidationResult resultValidation = new SmallUserValidator().Validate(_user);
                if (resultValidation.IsValid)
                {
                    UserDTO _userDto = new UserDTO()
                    {
                        name = Name,
                        cpf = Cpf.Replace(".","").Replace("-",""),
                        phone = Phone.Replace("(","").Replace(")","").Replace("-",""),
                        email = Email,
                        sex = Sex,
                        birthdate = Birthdate

                    };
                    Msg apiRetorno = await Util.GetApi().UpdateUser(_userDto);
                    if (apiRetorno.msg == Msg.SUCCESS)
                    {
                        await PageDialogService.DisplayAlertAsync("Atualizado", "Usuário Atualizado!", "OK");
                        await NavigationService.GoBackAsync();
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("Erro", apiRetorno.msg, "OK");
                    }
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Campos Requeridos:", string.Join("\n", resultValidation.Errors), "OK");
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