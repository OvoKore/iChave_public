namespace iChave.ModelDTO
{
    public class UpdateFcmDTO
    {
        public string email { get; set; }
        public string token_fcm { get; set; }
        public string cpf_cnpj { get; set; }
        public string role { get; set; }
        public UpdateFcmDTO()
        {
        }
        public UpdateFcmDTO(string _email, string _token_fcm)
        {
            email = _email;
            token_fcm = _token_fcm;
        }
    }
}
