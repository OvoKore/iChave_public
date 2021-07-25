using iChave.Model;
using iChave.ModelDTO;
using iChave.ViewModel.Base;
using Prism.Navigation;
using Prism.Services;
using System.Collections.ObjectModel;
using Firebase.Database;
using System.Reactive.Linq;
using System;
using Firebase.Database.Streaming;
using Prism.Commands;
using System.Threading.Tasks;
using iChave.Service;
using Refit;
using Realms;
using iChave.ModelRealm;
using System.Linq;
using System.Text.RegularExpressions;

namespace iChave.ViewModel.Chat
{
    public class ChatViewModel : ViewModelBase
    {
        protected ChatViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region Fields
        private FirebaseClient _firebase = new FirebaseClient(EndPoints.RealTimeUrl);
        public FirebaseClient firebase
        {
            get => _firebase;
            set => SetProperty(ref _firebase, value);
        }

        private ChatDTO _localChat;
        public ChatDTO LocalChat
        {
            get => _localChat;
            set => SetProperty(ref _localChat, value);
        }
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        private LocksmithListDTO _selectedChat;
        public LocksmithListDTO SelectedChat
        {
            get => _selectedChat;
            set => SetProperty(ref _selectedChat, value);
        }

        public ObservableCollection<ChatMessge> Chats { get; private set; } = new ObservableCollection<ChatMessge>();
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            LocalChat = parameters.GetValue<ChatDTO>("chat");
            Title = LocalChat.target_name;

            Realm realm = Realm.GetInstance();
            TokenFirebaseRealm token = realm.All<TokenFirebaseRealm>().FirstOrDefault();
            LocalChat.my_email = token.Email;
            LocalChat.role = token.Role;
            LocalChat.my_cpf_cnpj = token.CpfCnpj;

            int total_msgs = firebase
                .Child($"/{LocalChat.role}/{LocalChat.my_cpf_cnpj}/{LocalChat.target}")
                .OnceAsync<MsgDTO>()
                .Result.Count();

            if (total_msgs != 0)
            {
                SetObservable();
            }
            HasInitialized = true;
        }

        private void SetObservable()
        {
            firebase
                .Child($"/{LocalChat.role}/{LocalChat.my_cpf_cnpj}/{LocalChat.target}")
                .AsObservable<MsgDTO>()
                .Subscribe(d => ReceivedMessage(d));
        }
        private void ReceivedMessage(FirebaseEvent<MsgDTO> d)
        {
            string dt = Regex.Replace(d.Object.time, @"^(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2}).\d+$", "$4:$5:$6 $3/$2/$1");
            Chats.Add(new ChatMessge(d.Object.nome, d.Object.msg, dt));
        }

        private DelegateCommand sendMsgCommand;
        public DelegateCommand SendMsgCommand => sendMsgCommand ??= new DelegateCommand(async () => await SendMsgAsync(), () => !IsBusy);

        private async Task SendMsgAsync()
        {
            try
            {
                IsBusy = true;
                if (!string.IsNullOrEmpty(Text))
                {
                    IRestApi API = Util.GetApi();
                    try
                    {
                        LocalChat.msg = Text;
                        Text = string.Empty;
                        Msg usuariosRetorno = await API.SendChat(LocalChat);
                        if (usuariosRetorno.msg != Msg.SUCCESS)
                        {
                            await PageDialogService.DisplayAlertAsync("ERRO", "Erro ao enviar a mensagem.", "OK");
                        }
                    }
                    catch (ApiException ex)
                    {
                        await PageDialogService.DisplayAlertAsync(ex.StatusCode.ToString(), ex.GetContentAsAsync<Msg>().Result.msg, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await PageDialogService.DisplayAlertAsync("Erro", ex.Message, "OK");
            }
            finally
            {
                if (Chats.Count() == 0)
                {
                    SetObservable();
                }
                IsBusy = false;
            }
        }

        private DelegateCommand showCommand;
        public DelegateCommand ShowCommand => showCommand ??= new DelegateCommand(async () => await ShowAsync(), () => !IsBusy);

        private async Task ShowAsync()
        {
            try
            {
                IsBusy = true;
                string page = LocalChat.role == "user" ? "ShowLocksmithView": "ShowUserView";

                if (page == "ShowUserView")
                {
                    UserWithAddressDTO user = new UserWithAddressDTO() {
                        cpf = LocalChat.target
                    };
                    NavigationParameters parammeters = new NavigationParameters() {
                        { "user", user }
                    };
                    await NavigationWithParameters(page, parammeters);
                }
                else
                {
                    LocksmithListDTO locksmith = new LocksmithListDTO() {
                        cnpj = LocalChat.target
                    };
                    NavigationParameters parammeters = new NavigationParameters() {
                        { "locksmith", locksmith }
                    };
                    await NavigationWithParameters(page, parammeters);
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