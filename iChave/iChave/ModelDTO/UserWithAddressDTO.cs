using System;

namespace iChave.ModelDTO
{
    public class UserWithAddressDTO : AddressDTO
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string cpf { get; set; }
        public string sex { get; set; }
        public DateTime birthdate { get; set; }

        public UserWithAddressDTO()
        {
        }
    }
}
