using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Application.DTOs.Home
{
    public class ProductItemVersionDto
    {

        public required string Id { get; set; }
        [SwaggerSchema("Tên sản phẩm")]
        public required string Name { get; set; }
        [SwaggerSchema("Link ảnh")]
        public string? Image { get; set; }
        [SwaggerSchema("Giá")]
        public int Price { get; set; }
        [SwaggerSchema("Giá gạch")]
        public int? OldPrice { get; set; }
        [SwaggerSchema("Phiên bản mặc định")]
        public bool Selected { get; set; } = false;
    }
}