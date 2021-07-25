using iChave.Model;
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
using System.Text;
using System.Threading.Tasks;

namespace iChave.ViewModel.Service
{
    public class ServiceListLocksmithViewModel : ViewModelTabbedBase
    {
        protected ServiceListLocksmithViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region
        public ObservableCollection<ServiceDTO> Services { get; private set; } = new ObservableCollection<ServiceDTO>();
        #endregion
        private DelegateCommand newServiceCommand;
        public DelegateCommand NewServiceCommand => newServiceCommand ??= new DelegateCommand(async () => await Navigation("NewServiceView"), () => !IsBusy);

        public override void Initialize(INavigationParameters parameters)
        {
            GetServiceList();
            HasInitialized = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            ReloadRealm reload = new ReloadRealm("GetServiceList");
            if (reload.Get().Count() != 0)
            {
                GetServiceList();
                reload.Remove();
            }
        }

        public override void RefreshAsync()
        {
            GetServiceList();
            IsRefreshing = false;
        }

        public async void GetServiceList()
        {
            try
            {
                IsBusy = true;
                TokenFirebaseRealm token = Realm.GetInstance().All<TokenFirebaseRealm>().FirstOrDefault();
                List<ServiceDTO> apiRetorno = await Util.GetApi().GetLocksmithServices(token.Email);
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
                IsBusy = false;
            }
        }

        private DelegateCommand<ServiceDTO> editServiceCommand;
        public DelegateCommand<ServiceDTO> EditServiceCommand => editServiceCommand ??= new DelegateCommand<ServiceDTO>(async (service) => await EditServiceAsync(service), (service) => !IsBusy);

        private async Task EditServiceAsync(ServiceDTO service)
        {
            try
            {
                NavigationParameters navigationParams = new NavigationParameters
                {
                    { "Id", service.id },
                    { "Name", service.name },
                    { "Description", service.description },
                    { "LowPrice", service.low_price },
                    { "HighPrice", service.high_price }
                };
                await NavigationWithParameters("NewServiceView", navigationParams);
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                GetServiceList();
            }
        }

        private DelegateCommand<ServiceDTO> deleteServiceCommand;
        public DelegateCommand<ServiceDTO> DeleteServiceCommand => deleteServiceCommand ??= new DelegateCommand<ServiceDTO>(async (service) => await DeleteServiceAsync(service), (service) => !IsBusy);

        private async Task DeleteServiceAsync(ServiceDTO service)
        {
            try
            {
                bool result = await PageDialogService.DisplayAlertAsync("CONFIRMAR", "Você realmente quer deletar?", "Sim", "Não");
                if (result)
                {
                    IsBusy = true;
                    Msg apiRetorno = await Util.GetApi().DeleteService(service);
                    if (apiRetorno.msg == Msg.SUCCESS)
                    {
                        await PageDialogService.DisplayAlertAsync("Removido", "Removido com Sucesso.", "OK");
                    }
                    else
                    {
                        await PageDialogService.DisplayAlertAsync("Falha", "Falhou ao deletar.", "OK");
                    }
                    GetServiceList();
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