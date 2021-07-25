using FluentValidation.Results;
using iChave.Model;
using iChave.ModelDTO;
using iChave.ModelRealm;
using iChave.Service;
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
using System.Threading.Tasks;

namespace iChave.ViewModel.Configuration
{
    public class AddressViewModel : ViewModelBase
    {
        protected AddressViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private bool _back = false;
        public bool Back
        {
            get => _back;
            set => SetProperty(ref _back, value);
        }

        private List<State> _allEstados = Util.GetStates();
        public List<State> AllEstados
        {
            get => _allEstados;
            set => SetProperty(ref _allEstados, value);
        }
        private List<City> _allCidades = Util.GetCities();
        public List<City> AllCidades
        {
            get => _allCidades;
            set => SetProperty(ref _allCidades, value);
        }

        private List<City> _listCidade = new List<City>();
        public List<City> ListCidade
        {
            get => _listCidade;
            set => SetProperty(ref _listCidade, value);
        }

        private State _state = null;
        public State State
        {
            get => _state;
            set
            {
                SetProperty(ref _state, value);
                if (!Back)
                {
                    ListCidade = AllCidades.Where(c => c.State == value.ID).ToList();
                }
                City = null;
            }
        }
        private City _city = null;
        public City City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _cep;
        public string Cep
        {
            get => _cep;
            set => SetProperty(ref _cep, value);
        }

        private string _uf;
        public string Uf
        {
            get => _uf;
            set => SetProperty(ref _uf, value);
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

        private bool _enderecoHabilitado = true;
        public bool EnderecoHabilitado
        {
            get => _enderecoHabilitado;
            set => SetProperty(ref _enderecoHabilitado, value);
        }
        #endregion

        public override async void Initialize(INavigationParameters parameters)
        {
            try
            {
                IsBusy = true;
                TokenFirebaseRealm token = Realm.GetInstance().All<TokenFirebaseRealm>().First();
                AddressDTO _user = await Util.GetApi().GetAddress(token.Role, token.Email);
                if (_user != null)
                {
                    Cep = _user.cep;
                    Uf = _user.uf;
                    Cidade = _user.cidade;
                    Bairro = _user.bairro;
                    Logradouro = _user.logradouro;
                    Numero = _user.numero;
                    Complemento = _user.complemento;
                    if (!string.IsNullOrEmpty(Uf))
                    {
                        State = AllEstados.Where(x => x.StateAbbreviation == Uf).First();
                    }
                    if (!string.IsNullOrEmpty(Cidade))
                    {
                        City = AllCidades.Where(x => x.State == State.ID && x.Name == Cidade).First();
                    }
                    EnderecoHabilitado = false;
                }
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                HasInitialized = true;
            }
        }

        public override void Destroy()
        {
            Back = true;
        }

        private DelegateCommand consultarCepCommand;
        public DelegateCommand ConsultarCepCommand => consultarCepCommand ??= new DelegateCommand(async () => await ConsultarCepAsync(), () => !IsBusy);

        private async Task ConsultarCepAsync()
        {
            try
            {
                IsBusy = true;
                CepDTO cepResposta = await RestService.For<IRestApi>(EndPoints.CepUrl).Cep(Cep);
                if (cepResposta != null)
                {
                    State = AllEstados.First(e => e.StateAbbreviation == cepResposta.uf);
                    ListCidade = AllCidades.Where(c => c.State == State.ID).ToList();
                    City = ListCidade.First(c => c.Name == cepResposta.localidade);
                    Bairro = cepResposta.bairro;
                    Logradouro = cepResposta.logradouro;
                    EnderecoHabilitado = false;
                }
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
                EnderecoHabilitado = true;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private DelegateCommand registerCommand;
        public DelegateCommand RegisterCommand => registerCommand ??= new DelegateCommand(async () => await RegisterAsync(), () => !IsBusy);

        private async Task RegisterAsync()
        {
            try
            {
                IsBusy = true;
                AddressVal address = new AddressVal()
                {
                    cep = Cep,
                    uf = State.StateAbbreviation,
                    cidade = City.Name,
                    bairro = Bairro,
                    logradouro = Logradouro,
                    numero = Numero,
                    complemento = Complemento
                };
                ValidationResult resultValidation = new AddressValidator().Validate(address);
                if (resultValidation.IsValid)
                {
                    IRestApi api = Util.GetApi();
                    AddressDTO _address = new AddressDTO(address);
                    try
                    {
                        TokenFirebaseRealm token = Realm.GetInstance().All<TokenFirebaseRealm>().First();
                        Msg apiRetorno = await api.SetAddress(token.Role, token.Email, _address);
                        if (apiRetorno.msg == Msg.SUCCESS)
                        {
                            await PageDialogService.DisplayAlertAsync("Salvo", "Salvo com sucesso!", "OK");
                        }
                        else
                        {
                            await PageDialogService.DisplayAlertAsync("ERRO", apiRetorno.msg, "OK");
                        }
                        await NavigationService.GoBackAsync();
                    }
                    catch (ApiException ex)
                    {
                        await PageDialogService.DisplayAlertAsync(ex.StatusCode.ToString(), ex.GetContentAsAsync<Msg>().Result.msg, "OK");
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