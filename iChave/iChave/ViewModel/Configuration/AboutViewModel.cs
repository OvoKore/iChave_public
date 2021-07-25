using iChave.ViewModel.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Essentials;


namespace iChave.ViewModel.Configuration
{
    public class AboutViewModel : ViewModelBase
    {
        public string Version { get; private set; }
        protected AboutViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Version = "Versão: " + VersionTracking.CurrentVersion;
        }

        private DelegateCommand privacyCommand;
        public DelegateCommand PrivacyCommand => privacyCommand ??= new DelegateCommand(async () => await Navigation("PrivacyView"), () => !IsBusy);

        private DelegateCommand termsUseCommand;
        public DelegateCommand TermsUseCommand => termsUseCommand ??= new DelegateCommand(async () => await Navigation("TermsUseView"), () => !IsBusy);
    }
}
