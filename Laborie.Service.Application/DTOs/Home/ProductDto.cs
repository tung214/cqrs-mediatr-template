using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Application.DTOs.Home
{
    public class ProductDto
    {
        [SwaggerSchema("Danh sách sản phẩm cho đại lý")]
        public List<ProductItemDto>? Salon { get; set; }
        [SwaggerSchema("Danh sách sản phẩm mua lẻ")]
        public List<ProductItemDto>? Personal { get; set; }
    }
}