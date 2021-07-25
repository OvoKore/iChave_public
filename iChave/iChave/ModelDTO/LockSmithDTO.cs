using iChave.Validator;

namespace iChave.ModelDTO
{
    public class LocksmithDTO : LoginDTO
    {
        public string uid { get; set; }
        public string company_name { get; set; }
        public string phone { get; set; }
        public string cnpj { get; set; }
#nullable enable
        public string? state_registration { get; set; }
#nullable disable

        public LocksmithDTO()
        {
        }

        public LocksmithDTO(LocksmithVal _locksmith)
        {
            cnpj = _locksmith.cnpj.Replace(".", "").Replace("/", "").Replace("-", "");
            email = _locksmith.email.ToLower();
            password = Util.Crypt(_locksmith.password);
            company_name = _locksmith.company_name;
            phone = _locksmith.phone.Replace("(", "").Replace(")", "").Replace("-", "");
            state_registration = string.IsNullOrEmpty(_locksmith.state_registration) ? null : _locksmith.state_registration;
        }
    }
}