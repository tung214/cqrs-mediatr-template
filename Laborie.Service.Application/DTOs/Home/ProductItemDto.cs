using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Application.DTOs.Home
{
    public class ProductItemDto
    {
        public required string Id { get; set; }
        [SwaggerSchema("Tên sản phẩm")]
        public required string Name { get; set; }
        [SwaggerSchema("Danh sách ảnh sản phẩm")]
        public required List<string> Images { get; set; }
        [SwaggerSchema("Danh sách phiên bản")]
        public required List<ProductItemVersionDto> Versions { get; set; }
    }
}