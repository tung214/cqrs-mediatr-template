using FluentValidation;

namespace Laborie.Service.Application.Command.Login.Handlers
{
    public class ResendActiveCodeValidator : AbstractValidator<ResendActiveCode>
    {
        public ResendActiveCodeValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được rỗng!")
                .EmailAddress().WithMessage("Email không đúng định dạng!");
        }
    }
}