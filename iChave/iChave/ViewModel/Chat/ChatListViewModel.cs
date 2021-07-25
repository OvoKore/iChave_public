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

namespace iChave.ViewModel.Chat
{
    public class ChatListViewModel : ViewModelTabbedBase
    {
        protected ChatListViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
        }

        #region
        private ChatDTO _selectedChat;
        public ChatDTO SelectedChat
        {
            get => _selectedChat;
            set => SetProperty(ref _selectedChat, value);
        }

        public ObservableCollection<ChatDTO> Chats { get; private set; } = new ObservableCollection<ChatDTO>();
        #endregion

        public override void Initialize(INavigationParameters parameters)
        {
            GetChatList();
            HasInitialized = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (HasInitialized)
            {
                GetChatList();
            }
            else
            {
                SelectedChat = null;
            }
        }

        public override void RefreshAsync()
        {
            GetChatList();
            IsRefreshing = false;
        }

        public async void GetChatList()
        {
            try
            {
                IsBusy = true;
                Realm realm = Realm.GetInstance();
                TokenFirebaseRealm token = realm.All<TokenFirebaseRealm>().First();
                ChatDTO _getChatListDto = new ChatDTO()
                {
                    my_email = token.Email,
                    role = token.Role
                };
                List<ChatDTO> apiRetorno = await Util.GetApi().GetChatList(_getChatListDto);
                Chats.Clear();
                foreach (ChatDTO s in apiRetorno)
                {
                    ChatDTO sel = new ChatDTO()
                    {
                        target_name = s.target_name,
                        target = s.target
                    };
                    int findChatLocal = realm.All<ChatRealm>().Count();
                    if (findChatLocal == 0)
                    {
                        realm.Write(() =>
                        {
                            realm.Add(new ChatRealm()
                            {
                                target = s.target,
                                name = s.target_name
                            });
                        });
                    }
                    Chats.Add(sel);
                }
                List<ChatRealm> listLocalChat = realm.All<ChatRealm>().ToList();
                foreach (ChatRealm c in listLocalChat)
                {
                    bool novo = true;
                    foreach(ChatDTO s in apiRetorno)
                    {
                        if (s.target == c.target)
                        {
                            novo = false;
                            break;
                        }
                    }
                    if (novo)
                    {
                        ChatDTO sel = new ChatDTO()
                        {
                            target_name = c.name,
                            target = c.target
                        };
                        Chats.Add(sel);
                    }
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

        private DelegateCommand<ChatDTO> editServiceCommand;
        public DelegateCommand<ChatDTO> EditServiceCommand => editServiceCommand ??= new DelegateCommand<ChatDTO>(async (chat) => await OpenLocksmithAsync(chat), (chat) => !IsBusy);

        private async Task OpenLocksmithAsync(ChatDTO chat)
        {
            try
            {
                IsBusy = true;
                NavigationParameters navigationParams = new NavigationParameters
                {
                    { "chat", chat }
                };
                await NavigationWithParameters("ChatView", navigationParams);
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
