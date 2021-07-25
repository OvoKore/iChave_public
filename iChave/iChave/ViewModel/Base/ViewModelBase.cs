using System;
using System.Threading.Tasks;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;

namespace iChave.ViewModel.Base
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible, IActiveAware
    {
        protected INavigationService NavigationService { get; private set; }

        protected IPageDialogService PageDialogService { get; set; }

        public event EventHandler IsActiveChanged;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (SetProperty(ref isBusy, value))
                    IsNotBusy = !isBusy;
            }
        }

        bool isNotBusy = true;
        public bool IsNotBusy
        {
            get => isNotBusy;
            set
            {
                if (SetProperty(ref isNotBusy, value))
                    IsBusy = !isNotBusy;
            }
        }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                SetProperty(ref isRefreshing, value);
                IsBusy = isRefreshing;
            }
        }

        private bool hasInitialized = false;
        public bool HasInitialized
        {
            get => hasInitialized;
            set => SetProperty(ref hasInitialized, value);
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value, RaiseIsActiveChanged);
        }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        protected ViewModelBase(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;

            Title = $"Default";
        }

        // Called before the View (Xamarin.Forms Page) is pushed onto the Navigation Stack
        public virtual void Initialize(INavigationParameters parameters)
        {
            HasInitialized = true;
        }

        // Called when the View is Navigated away from
        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        // Called any time the View is is Navigated to, or back to... 
        // and AFTER Initialize...
        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }

        protected virtual void RaiseIsActiveChanged()
        {
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }

        private async Task NavigationAsync(string view)
        {
            try
            {
                IsBusy = true;
                await NavigationService.NavigateAsync(view);
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

        protected async Task Navigation(string view)
        {
            await NavigationAsync(view);
        }

        private async Task NavigationWithParametersAsync(string view, NavigationParameters parameters)
        {
            try
            {
                IsBusy = true;
                await NavigationService.NavigateAsync(view, parameters);
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

        protected async Task NavigationWithParameters(string view, NavigationParameters parameters)
        {
            await NavigationWithParametersAsync(view, parameters);
        }

        private DelegateCommand refreshCommand;
        public DelegateCommand RefreshCommand => refreshCommand ??= new DelegateCommand(() => RefreshAsync(), () => !IsBusy);

        public virtual void RefreshAsync()
        {
            IsRefreshing = false;
        }
    }
}