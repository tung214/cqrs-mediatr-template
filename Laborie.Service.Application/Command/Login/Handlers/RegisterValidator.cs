using FluentValidation;

namespace Laborie.Service.Application.Command.Login.Handlers;
public sealed class RegisterValidator : AbstractValidator<Register>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được rỗng!");

        RuleFor(x => x.Email)
               .NotEmpty().WithMessage("Email không được rỗng!")
               .EmailAddress().WithMessage("Email không đúng định dạng!");        
    }
}
