namespace iChave.Service
{
    public static class EndPoints
    {

#if DEBUG
        public static string BaseUrl => "http://192.168.0.59";
#else
        public static string BaseUrl => "https://ichave.web.app";
#endif
        public static string RealTimeUrl => "https://msgchatichave.firebaseio.com/";
        public static string CepUrl => "http://viacep.com.br/ws";
    }
}