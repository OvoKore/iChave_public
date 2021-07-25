using iChave.ViewModel.Base;
using Prism.Navigation;
using Prism.Services;

namespace iChave.ViewModel
{
    class InitialViewModel : ViewModelBase
    {
        protected InitialViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await Navigation(Util.GetInitialViewAsync());
        }
    }
}
