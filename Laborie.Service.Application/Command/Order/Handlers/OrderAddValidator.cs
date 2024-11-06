using FluentValidation;

namespace Laborie.Service.Application.Command.Order.Handlers
{
    public class OrderAddValidator : AbstractValidator<OrderAdd>
    {
        public OrderAddValidator()
        {
            RuleFor(x => x.AddressId).NotEmpty().WithMessage("Địa chỉ không được trống");
            RuleFor(x => x.OrderItems).Must(x => x.Count == 0).WithMessage("Sản phẩm trong đơn không được trống");
            RuleForEach(x => x.OrderItems).Must(ValidOrderItem).WithMessage("Thông tin sản phẩm trong đơn hàng không hợp lệ");
        }

        private bool ValidOrderItem(OrderItem item)
        {
            return !string.IsNullOrEmpty(item.ProductId) && !string.IsNullOrEmpty(item.VariantId) && item.Quantity > 0;
        }
    }
}