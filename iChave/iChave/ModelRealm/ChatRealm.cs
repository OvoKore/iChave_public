using Realms;

namespace iChave.ModelRealm
{
    public class ChatRealm : RealmObject
    {
        public string target { get; set; }
        public string name { get; set; }

        public ChatRealm()
        {
        }

        public ChatRealm(string _uid, string _name)
        {
            target = _uid;
            name = _name;
        }
    }
}
