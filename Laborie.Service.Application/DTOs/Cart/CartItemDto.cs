using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Application.DTOs.Cart
{
    [SwaggerSchema("Thông tin giỏ hàng")]
    public class CartItemDto
    {
        public required string Id { get; set; }
        [SwaggerSchema("Tên sản phẩm")]
        public required string Name { get; set; }
        [SwaggerSchema("Link ảnh")]
        public string? Image { get; set; }
        [SwaggerSchema("Tên phiên bản: 100ml, 20g...")]
        public string? VersionName { get; set; }
        [SwaggerSchema("Số lượng")]
        public int Quantity { get; set; }
        [SwaggerSchema("Giá")]
        public int Price { get; set; }
    }
}