using Realms;

namespace iChave.ModelRealm
{
    public class TokenFCMRealm : RealmObject
    {
        public string Token { get; set; }

        public TokenFCMRealm()
        {
        }
        public TokenFCMRealm(string _token)
        {
            Token = _token;
        }
    }
}
