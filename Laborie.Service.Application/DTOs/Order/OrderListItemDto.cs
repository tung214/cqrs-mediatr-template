using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Application.DTOs.Order
{
    public class OrderListItemDto
    {
        public required string Id { get; set; }
        [SwaggerSchema("Trạng thái xác nhận đơn hàng")]
        public bool IsConfirm { get; set; } = false;
        [SwaggerSchema("Trạng thái giao hàng")]
        public bool IsShipping { get; set; } = false;
        [SwaggerSchema("Trạng thái hủy đơn hàng")]
        public bool IsCancel { get; set; } = false;
        [SwaggerSchema("Trạng thái nhận đơn hàng")]
        public bool IsDelivering { get; set; } = false;
        [SwaggerSchema("Trạng thái hoàn tất đơn hàng")]
        public bool IsComplete { get; set; } = false;
        [SwaggerSchema("Trạng thái đơn hàng")]
        public required string Status { get; set; }
        [SwaggerSchema("Tên sản phẩm đầu tiên trong đơn hàng")]
        public required string Name { get; set; }
        [SwaggerSchema("Tổng tiền")]
        public int TotalPrice { get; set; }
        [SwaggerSchema("Số lượng sản phẩm trong đơn hàng")]
        public int NumberItems { get; set; }
    }
}