using Realms;

namespace iChave.ModelRealm
{
    public class TokenFirebaseRealm : RealmObject
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string CpfCnpj { get; set; }
        public string FirebaseToken { get; set; }
        public string RefreshToken { get; set; }
        
        public TokenFirebaseRealm()
        {
        }
    }
}
