using iChave.Validator;

namespace iChave.ModelDTO
{
    public class AddressDTO
    {
        public string cep { get; set; }
        public string uf { get; set; }
        public string cidade { get; set; }
        public string bairro { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }

        public AddressDTO()
        {
        }

        public AddressDTO(AddressVal _addressValidator)
        {
            cep = _addressValidator.cep;
            uf = _addressValidator.uf;
            cidade = _addressValidator.cidade;
            bairro = _addressValidator.bairro;
            logradouro = _addressValidator.logradouro;
            numero = _addressValidator.numero;
            complemento = _addressValidator.complemento;
        }
    }
}
