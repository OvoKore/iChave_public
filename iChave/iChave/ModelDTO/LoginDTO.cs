namespace iChave.ModelDTO
{
    public class LoginDTO
    {
        public string email { get; set; }
        public string password { get; set; }

        public LoginDTO()
        {
        }

        public LoginDTO(string _email, string _password)
        {
            email = _email.ToLower();
            password = Util.Crypt(_password);
        }
    }
}
