using FluentValidation;

namespace iChave.Validator
{
    public class LocksmithPassword
    {
        public string current_password { get; set; }
        public string new_password { get; set; }
        public string confirm_password { get; set; }
    }

    public class LocksmithPasswordValidator : AbstractValidator<LocksmithPassword>
    {
        public LocksmithPasswordValidator()
        {
            RuleFor(x => x.current_password).Must(Util.PasswordValidator).NotNull();
            RuleFor(x => x.new_password).Must(Util.PasswordValidator).NotNull();
            RuleFor(x => x.confirm_password).Equal(x => x.new_password).NotNull();
        }
    }

    public class SmallLocksmithVal
    {
        public string email { get; set; }
        public string company_name { get; set; }
        public string phone { get; set; }
        public string cnpj { get; set; }
        public string state_registration { get; set; }
    }

    public class SmallLocksmithValidator : AbstractValidator<SmallLocksmithVal>
    {
        public SmallLocksmithValidator()
        {
            RuleFor(x => x.company_name).Length(2, 50).NotNull();
            RuleFor(x => x.phone).Length(14).NotNull();
            RuleFor(x => x.cnpj).Must(Util.CnpjValidator).NotNull();
            RuleFor(x => x.email).EmailAddress().NotNull();
            RuleFor(x => x.state_registration).Length(2, 50).When(x => !string.IsNullOrEmpty(x.state_registration));
        }
    }

    public class LocksmithVal : SmallLocksmithVal
    {
        public string confirm_email { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }

    public class LocksmithValidator : AbstractValidator<LocksmithVal>
    {
        public LocksmithValidator()
        {
            RuleFor(x => x.company_name).Length(2, 50).NotNull();
            RuleFor(x => x.phone).Length(14).NotNull();
            RuleFor(x => x.cnpj).Must(Util.CnpjValidator).NotNull();
            RuleFor(x => x.password).Must(Util.PasswordValidator).NotNull();
            RuleFor(x => x.confirm_password).Equal(x => x.password).NotNull();
            RuleFor(x => x.email).EmailAddress().NotNull();
            RuleFor(x => x.confirm_email).Equal(x => x.email).NotNull();
            RuleFor(x => x.state_registration).Length(2, 50).When(x => !string.IsNullOrEmpty(x.state_registration));
        }
    }
}