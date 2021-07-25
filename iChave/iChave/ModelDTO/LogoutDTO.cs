namespace iChave.ModelDTO
{
    public class LogoutDTO
    {
        public string email { get; set; }
        public string role { get; set; }

        public LogoutDTO()
        {
        }

        public LogoutDTO(string _email, string _role)
        {
            email = _email.ToLower();
            role = _role;
        }
    }
}
