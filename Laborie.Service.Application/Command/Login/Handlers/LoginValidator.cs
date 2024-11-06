using FluentValidation;

namespace Laborie.Service.Application.Command.Login.Handlers
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.DeviceToken).NotEmpty().WithMessage("Device không được rỗng");
            RuleFor(x => x.Email)
             .NotEmpty().WithMessage("Email không được rỗng!")
             .EmailAddress().WithMessage("Email không đúng định dạng!");

            RuleFor(x => x.Password)
               .NotEmpty().WithMessage("Mật khẩu không được rỗng")
               .MinimumLength(8).WithMessage("Mật khẩu phải gồm 8-16 ký tự.")
               .MaximumLength(16).WithMessage("Mật khẩu phải gồm 8-16 ký tự.")
               .Matches(@"[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất 1 ký tự viết hoa, 1 ký tự viết thường, 1 ký tự số, 1 ký tự đặc biệt!")
               .Matches(@"[a-z]").WithMessage("Mật khẩu phải chứa ít nhất 1 ký tự viết hoa, 1 ký tự viết thường, 1 ký tự số, 1 ký tự đặc biệt!")
               .Matches(@"\d").WithMessage("Mật khẩu phải chứa ít nhất 1 ký tự viết hoa, 1 ký tự viết thường, 1 ký tự số, 1 ký tự đặc biệt!")
               .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]").WithMessage("Mật khẩu phải chứa ít nhất 1 ký tự viết hoa, 1 ký tự viết thường, 1 ký tự số, 1 ký tự đặc biệt!")
               .Matches("^[^£# “”]*$").WithMessage("Mật khẩu không chứa ký tự £ # “” hoặc dấu cách");
        }
    }
}