using System;
using iChave.Validator;

namespace iChave.ModelDTO
{
    public class UserDTO : LoginDTO
    {
        public string uid { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string cpf { get; set; }
        public string sex { get; set; }
        public DateTime birthdate { get; set; }

        public UserDTO()
        {
        }

        public UserDTO(UserVal _user)
        {
            name = _user.name;
            cpf = _user.cpf.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty);
            phone = _user.phone.Replace("(", string.Empty).Replace(")", string.Empty).Replace("-", string.Empty);
            sex = _user.sex;
            birthdate = _user.birthdate;
            email = _user.email.ToLower();
            password = Util.Crypt(_user.password);
        }
    }
}