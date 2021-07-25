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
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iChave.ViewModel.Service
{
    public class NewServiceViewModel : ViewModelBase
    {
        protected NewServiceViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private string _id;
        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private double _lowPrice;
        public double LowPrice
        {
            get => _lowPrice;
            set => SetProperty(ref _lowPrice, value);
        }

        private double _highPrice;
        public double HighPrice
        {
            get => _highPrice;
            set => SetProperty(ref _highPrice, value);
        }
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.Count() != 0)
            {
                Id = parameters.GetValue<string>("Id");
                Name = parameters.GetValue<string>("Name");
                Description = parameters.GetValue<string>("Description");
                LowPrice = parameters.GetValue<double>("LowPrice");
                HighPrice = parameters.GetValue<double>("HighPrice");
            }
            HasInitialized = true;
        }

        private DelegateCommand registerCommand;
        public DelegateCommand RegisterCommand => registerCommand ??= new DelegateCommand(async () => await RegisterAsync(), () => !IsBusy);

        private async Task RegisterAsync()
        {
            try
            {
                IsBusy = true;
                ServiceVal service = new ServiceVal()
                {
                    name = Name,
                    description = Description,
                    low_price = LowPrice,
                    high_price = HighPrice
                };
                ValidationResult resultValidation = new ServiceValidator().Validate(service);
                if (resultValidation.IsValid)
                {
                    iChave.Service.IRestApi api = Util.GetApi();
                    ServiceDTO _service = new ServiceDTO(service);
                    try
                    {
                        if (string.IsNullOrEmpty(Id))
                        {
                            TokenFirebaseRealm token = Realm.GetInstance().All<TokenFirebaseRealm>().FirstOrDefault();
                            Msg apiRetorno = await api.AddService(token.Email, _service);
                            if (apiRetorno.msg == Msg.SUCCESS)
                            {
                                await PageDialogService.DisplayAlertAsync("", "Adicionado com sucesso!", "OK");
                            }
                        }
                        else
                        {
                            _service.id = Id;
                            Msg apiRetorno = await api.UpdateService(_service);
                            if (apiRetorno.msg == Msg.SUCCESS)
                            {
                                await PageDialogService.DisplayAlertAsync("", "Atualizado com sucesso!", "OK");
                            }
                        }

                        new ReloadRealm("GetServiceList").Add();
                        await NavigationService.GoBackAsync();
                    }
                    catch (ApiException ex)
                    {
                        await PageDialogService.DisplayAlertAsync(ex.StatusCode.ToString(), ex.GetContentAsAsync<MsgDTO>().Result.msg, "OK");
                    }
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("Required fields", string.Join("\n", resultValidation.Errors), "OK");
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
