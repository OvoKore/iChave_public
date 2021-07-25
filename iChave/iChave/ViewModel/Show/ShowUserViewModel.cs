using iChave.ModelDTO;
using iChave.ViewModel.Base;
using Prism.Navigation;
using Prism.Services;

namespace iChave.ViewModel.Show
{
    public class ShowUserViewModel : ViewModelBase
    {
        protected ShowUserViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region
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
        #endregion

        public async override void Initialize(INavigationParameters parameters)
        {
            UserWithAddressDTO user = parameters.GetValue<UserWithAddressDTO>("user");
            user = await Util.GetApi().GetUserWithAddres(user.cpf);

            Title = user.name;
            Name = user.name;
            Cidade = user.cidade;
            Bairro = user.bairro;
            Logradouro = user.logradouro;
            Numero = user.numero;
            Complemento = user.complemento;

            HasInitialized = true;
        }
    }
}
