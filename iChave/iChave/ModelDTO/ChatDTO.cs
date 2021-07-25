using iChave.Model;

namespace iChave.ModelDTO
{
    public class ChatDTO : Msg
    {
        public string my_email { get; set; }
        public string my_cpf_cnpj { get; set; }
        public string role { get; set; }
        public string target { get; set; }
        public string target_name { get; set; }

        public ChatDTO() { }
    }
}
