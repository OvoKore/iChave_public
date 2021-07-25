using FluentValidation;

namespace iChave.Validator
{
    public class ServiceVal
    {
        public string name { get; set; }
        public string description { get; set; }
        public double low_price { get; set; }
        public double high_price { get; set; }
    }

    public class ServiceValidator : AbstractValidator<ServiceVal>
    {
        public ServiceValidator()
        {
            RuleFor(x => x.name).Length(2, 50).NotNull();
            RuleFor(x => x.description).Length(2, 100).NotNull();
            RuleFor(x => x.low_price).LessThanOrEqualTo(y => y.high_price);
            RuleFor(x => x.high_price).GreaterThanOrEqualTo(y => y.low_price);
        }
    }
}