using iChave.ModelDTO;
using iChave.ModelRealm;
using iChave.ViewModel.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace iChave.ViewModel.Show
{
    public class ShowLocksmithViewModel : ViewModelBase
    {
        protected ShowLocksmithViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region
        public LocksmithListDTO Locksmith { get; private set; } = new LocksmithListDTO();

        private string _company_name;
        public string CompanyName
        {
            get => _company_name;
            set => SetProperty(ref _company_name, value);
        }

        private string _cidade;
        public string Cidade
        {
            get => _cidade;
            set => SetProperty(ref _cidade, value);
        }

        private string _bairro;
        public string Bairro
        {
            get => _bairro;
            set => SetProperty(ref _bairro, value);
        }

        private string _logradouro;
        public string Logradouro
        {
            get => _logradouro;
            set => SetProperty(ref _logradouro, value);
        }

        private string _numero;
        public string Numero
        {
            get => _numero;
            set => SetProperty(ref _numero, value);
        }

        private string _complemento;
        public string Complemento
        {
            get => _complemento;
            set => SetProperty(ref _complemento, value);
        }

        private ServiceDTO _selectedService;
        public ServiceDTO SelectedService
        {
            get => _selectedService;
            set => SetProperty(ref _selectedService, value);
        }

        public ObservableCollection<ServiceDTO> Services { get; private set; } = new ObservableCollection<ServiceDTO>();
        #endregion

        public async override void Initialize(INavigationParameters parameters)
        {
            Locksmith = parameters.GetValue<LocksmithListDTO>("locksmith");
            if (string.IsNullOrEmpty(Locksmith.company_name))
            {
                Locksmith = await Util.GetApi().GetLocksmithListByCnpj(Locksmith.cnpj);
            }
            Title = Locksmith.company_name;
            CompanyName = Locksmith.company_name;
            Cidade = Locksmith.cidade;
            Bairro = Locksmith.bairro;
            Logradouro = Locksmith.logradouro;
            Numero = Locksmith.numero;
            Complemento = Locksmith.complemento;

            GetLocksmithServices();
            HasInitialized = true;
        }

        public override void RefreshAsync()
        {
            GetLocksmithServices();
            IsRefreshing = false;
        }

        public async void GetLocksmithServices()
        {
            try
            {
                IsBusy = true;
                List<ServiceDTO> apiRetorno = await Util.GetApi().GetLocksmithServices(Locksmith.email);
                Services.Clear();
                foreach (ServiceDTO s in apiRetorno)
                {
                    Services.Add(s);
                }
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                SelectedService = null;
                IsBusy = false;
            }
        }

        private DelegateCommand<ServiceDTO> showFeedbackCommand;
        public DelegateCommand<ServiceDTO> ShowFeedbackCommand => showFeedbackCommand ??= new DelegateCommand<ServiceDTO>(async (service) => await ShowFeedbackAsync(service), (service) => !IsBusy);

        private async Task ShowFeedbackAsync(ServiceDTO service)
        {
            try
            {
                NavigationParameters navigationParams = new NavigationParameters
                {
                    { "Id", service.id }
                };
                //await NavigationWithParameters("FeedbackView", navigationParams);
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                SelectedService = null;
                IsBusy = false;
            }
        }

        private DelegateCommand chatCommand;
        public DelegateCommand ChatCommand => chatCommand ??= new DelegateCommand(async () => await ChatAsync(), () => !IsBusy);

        private async Task ChatAsync()
        {
            try
            {
                IsBusy = true;
                Realm realm = Realm.GetInstance();
                int findChat = realm.All<ChatRealm>().Where(x => x.target == Locksmith.cnpj).Count();
                if (findChat == 0)
                {
                    realm.Write(() =>
                    {
                        realm.Add(new ChatRealm()
                        {
                            target = Locksmith.cnpj,
                            name = CompanyName
                        });
                    });
                    await PageDialogService.DisplayAlertAsync("", $"Chat com {CompanyName} adicionado a lista de chat", "OK");
                }
                else
                {
                    await PageDialogService.DisplayAlertAsync("", $"Você já tem {CompanyName} na sua lista de chat", "OK");
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
