using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Realms;
using Refit;
using iChave.ModelRealm;
using iChave.Service;
using iChave.Model;
using Firebase.Auth;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Plugin.FirebasePushNotification;

namespace iChave
{
    public static class Util
    {
        public static List<string> SexList = new List<string>() { "Masculino", "Feminino", "Outro" };

        public static bool EmailValidator(string Email)
        {
            string emailRegex = @"(^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$)";
            return Regex.IsMatch(Email, emailRegex);
        }

        public static bool PasswordValidator(string Senha)
        {
            string passwordRegex = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";
            return Regex.IsMatch(Senha, passwordRegex);
        }

        public static bool CnpjValidator(string Cnpj)
        {
            if (string.IsNullOrEmpty(Cnpj))
                return false;

            Cnpj = Cnpj.Trim();
            Cnpj = Cnpj.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            if (Cnpj.Length != 14)
                return false;

            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;

            tempCnpj = Cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito += resto.ToString();
            return Cnpj.EndsWith(digito);
        }

        public static bool CpfValidator(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", string.Empty).Replace("-", string.Empty);
            if (cpf.Length != 11)
                return false;

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito += resto.ToString();
            return cpf.EndsWith(digito);
        }

        public static string Crypt(string password)
        {
            string hexString = BitConverter.ToString(Encoding.Default.GetBytes(password)).Replace("-", string.Empty);
            char[] listChar = hexString.ToCharArray();
            Array.Reverse(listChar);
            return new string(listChar);
        }

        public static IRestApi GetApi()
        {
            return RestService.For<IRestApi>(EndPoints.BaseUrl);
        }

        public static List<State> GetStates()
        {
            Assembly assembly = typeof(Util).GetTypeInfo().Assembly;
            string stateJson = "State.json";
            Stream streamState = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Json.{stateJson}");
            StreamReader readerState = new StreamReader(streamState);
            string jsonStringState = readerState.ReadToEnd();
            List<State> JsonEstado = JsonConvert.DeserializeObject<List<State>>(jsonStringState);
            return JsonEstado;
        }

        public static List<City> GetCities()
        {
            Assembly assembly = typeof(Util).GetTypeInfo().Assembly;
            string cityJson = "City.json";
            Stream streamCity = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.Json.{cityJson}");
            StreamReader readerCity = new StreamReader(streamCity);
            string jsonStringCity = readerCity.ReadToEnd();
            List<City> JsonCidade = JsonConvert.DeserializeObject<List<City>>(jsonStringCity);
            return JsonCidade;
        }

        public static string GetInitialViewAsync()
        {
            string go = "/NavigationPage/LoginView";
            Realm realm = Realm.GetInstance();
            TokenFirebaseRealm token = realm.All<TokenFirebaseRealm>().FirstOrDefault();
            if (token != null)
            {
                go = token.Role == "user" ? "/NavigationPage/MenuUserView" : "/NavigationPage/MenuLocksmithView";
            }
            return go;
        }

        public static FirebaseAuthProvider GetAuthFirebase()
        {
            string key = Device.RuntimePlatform == Device.Android ? FirebaseKey.FirebaseAndroidWebAPIkey : FirebaseKey.FirebaseAndroidWebAPIkey;
            return new FirebaseAuthProvider(new FirebaseConfig(key));
        }

        public static SnackBarOptions GetSnackBarOptions(FirebasePushNotificationDataEventArgs e)
        {
            SnackBarOptions options = new SnackBarOptions
            {
                MessageOptions = new MessageOptions
                {
                    Foreground = Color.White,
                    Message = $"{e.Data["title"]}: {e.Data["body"]}"
                },
                BackgroundColor = Color.Black,
                Duration = System.TimeSpan.FromSeconds(5),
            };
            return options;
        }
    }
}