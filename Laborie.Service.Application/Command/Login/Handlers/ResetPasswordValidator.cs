using FluentValidation;

namespace Laborie.Service.Application.Command.Login.Handlers
{
    public class ResetPasswordValidator : AbstractValidator<ResetPassword>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Email)
              .NotEmpty().WithMessage("Email không được rỗng!")
              .EmailAddress().WithMessage("Email không đúng định dạng!");
        }
    }
}