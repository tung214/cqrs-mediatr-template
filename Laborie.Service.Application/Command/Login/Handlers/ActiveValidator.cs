using FluentValidation;

namespace Laborie.Service.Application.Command.Login.Handlers
{
    public class ActiveValidator : AbstractValidator<Active>
    {
        public ActiveValidator()
        {
            RuleFor(x => x.Email)
              .NotEmpty().WithMessage("Email không được rỗng!")
              .EmailAddress().WithMessage("Email không đúng định dạng!");
            RuleFor(x => x.ActiveCode).NotEmpty();
        }
    }
}