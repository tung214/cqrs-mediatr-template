using FluentValidation;

namespace Laborie.Service.Application.Command.Profile.Handlers
{
    public class ProfileUpdateValidator : AbstractValidator<ProfileUpdate>
    {
        public ProfileUpdateValidator()
        {
            RuleFor(x => x.Phone).Must(ValidPhone).WithMessage("Số điện thoại không hợp lệ!");
            RuleFor(x => x.DateOfBirth).Must(ValidDateOfBirth).WithMessage("Ngày sinh không được nhỏ hơn 16 tuổi");
        }

        private bool ValidPhone(string? phone)
        {
            if (string.IsNullOrEmpty(phone)) return true;

            return phone.StartsWith('0') && phone.Length == 10;
        }

        private bool ValidDateOfBirth(DateTime? date)
        {
            if (date == null) return true;

            return DateTime.Today.AddYears(-16) <= date;
        }
    }
}