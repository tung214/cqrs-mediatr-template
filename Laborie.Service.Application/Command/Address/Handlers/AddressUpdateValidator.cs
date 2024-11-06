using FluentValidation;

namespace Laborie.Service.Application.Command.Address.Handlers
{
    public class AddressUpdateValidator : AbstractValidator<AddressUpdate>
    {
        public AddressUpdateValidator()
        {
            RuleFor(x => x.AddressId).NotEmpty().WithMessage("Địa chỉ thay đổi không hợp lệ!");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên không được để trống!");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Số điện thoại không được để trống!");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ không được để trống!");
            RuleFor(x => x.ProvinceId).NotEmpty().WithMessage("Tỉnh thành phố không được để trống!");
            RuleFor(x => x.DistrictId).NotEmpty().WithMessage("Quận huyện không được để trống!");
            RuleFor(x => x.WardId).NotEmpty().WithMessage("Xã, thị trấn không được để trống!");
        }
    }
}