using iChave.Model;
using iChave.ViewModel.Base;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace iChave.ViewModel.LoginSignUp
{
    public class ForgetPasswordViewModel : ViewModelBase
    {
        protected ForgetPasswordViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService, pageDialogService)
        {
            Email = string.Empty;
        }

        #region Fields
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        #endregion

        private DelegateCommand sendResetCommand;
        public DelegateCommand SendResetCommand => sendResetCommand ??= new DelegateCommand(async () => await SendResetAsync(), () => !IsBusy);

        private async Task SendResetAsync()
        {
            try
            {
                IsBusy = true;
                if (!string.IsNullOrEmpty(Email.ToLower()))
                {
                    if (Util.EmailValidator(Email.ToLower()))
                    {
                        try
                        {
                            Msg apiRetorno = await Util.GetApi().SendPasswordReset(new Dictionary<string, string>() { { "email", Email.ToLower() } });
                            if (apiRetorno.msg == Msg.SUCCESS)
                            {
                                await PageDialogService.DisplayAlertAsync("", "Email enviado", "OK");
                                await NavigationService.GoBackAsync();
                            }
                            else
                            {
                                await PageDialogService.DisplayAlertAsync("ERRO", "Erro ao enviar o email.", "OK");
                            }
                        }
                        catch (ApiException ex)
                        {
                            await PageDialogService.DisplayAlertAsync(ex.StatusCode.ToString(), ex.GetContentAsAsync<Msg>().Result.msg, "OK");
                        }
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
    }
}
