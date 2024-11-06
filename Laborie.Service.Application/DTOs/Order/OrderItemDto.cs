using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Application.DTOs.Order
{
    public class OrderItemDto
    {
        [SwaggerSchema("Id sản phẩm")]
        public required string ProductId { get; set; }
        [SwaggerSchema("Tên sản phẩm")]
        public required string ProductName { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}