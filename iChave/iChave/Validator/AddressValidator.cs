using FluentValidation;

namespace iChave.Validator
{
    public class AddressVal
    {
        public string cep { get; set; }
        public string uf { get; set; }
        public string cidade { get; set; }
        public string bairro { get; set; }
        public string logradouro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
    }

    public class AddressValidator : AbstractValidator<AddressVal>
    {
        public AddressValidator()
        {
            RuleFor(x => x.cep).Length(9).NotNull();
            RuleFor(x => x.uf).Length(2).NotNull();
            RuleFor(x => x.cidade).Length(2, 50).NotNull();
            RuleFor(x => x.bairro).Length(2, 100).NotNull();
            RuleFor(x => x.logradouro).Length(2, 100).NotNull();
            RuleFor(x => x.numero).Length(1, 20).NotNull();
            RuleFor(x => x.complemento).Length(0, 50);
        }
    }
}
