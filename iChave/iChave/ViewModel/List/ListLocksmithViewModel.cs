using iChave.ModelDTO;
using iChave.ViewModel.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace iChave.ViewModel.List
{
    public class ListLocksmithViewModel : ViewModelTabbedBase
    {
        protected ListLocksmithViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region
        private LocksmithListDTO _selectedLocksmith;
        public LocksmithListDTO SelectedLocksmith
        {
            get => _selectedLocksmith;
            set => SetProperty(ref _selectedLocksmith, value);
        }

        public ObservableCollection<LocksmithListDTO> Locksmiths { get; private set; } = new ObservableCollection<LocksmithListDTO>();
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            GetLocksmithList();
            HasInitialized = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (HasInitialized)
            {
                GetLocksmithList();
            }
        }

        public override void RefreshAsync()
        {
            GetLocksmithList();
            IsRefreshing = false;
        }

        public async void GetLocksmithList()
        {
            try
            {
                IsBusy = true;
                List<LocksmithListDTO> apiRetorno = await Util.GetApi().GetLocksmithList();
                Locksmiths.Clear();
                foreach (LocksmithListDTO s in apiRetorno)
                {
                    Locksmiths.Add(s);
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

        private DelegateCommand<LocksmithListDTO> editServiceCommand;
        public DelegateCommand<LocksmithListDTO> EditServiceCommand => editServiceCommand ??= new DelegateCommand<LocksmithListDTO>(async (locksmith) => await OpenLocksmithAsync(locksmith), (locksmith) => !IsBusy);

        private async Task OpenLocksmithAsync(LocksmithListDTO locksmith)
        {
            try
            {
                IsBusy = true;
                NavigationParameters parammeters = new NavigationParameters() {
                    { "locksmith", locksmith }
                };
                await NavigationWithParameters("ShowLocksmithView", parammeters);
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                SelectedLocksmith = null;
                IsBusy = false;
            }
        }
    }
}