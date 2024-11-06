using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Application.DTOs.Address
{
    public class AddressItemDto
    {
        public required string Id { get; set; }
        [SwaggerSchema("Tên địa chỉ")]
        public required string Name { get; set; }
        [SwaggerSchema("Số điện thoại nhận hàng")]
        public string? Phone { get; set; }
        [SwaggerSchema("Địa chỉ")]
        public required string Address { get; set; }
        [SwaggerSchema("Tên tỉnh/thành phố")]
        public required string ProvinceName { get; set; }
        [SwaggerSchema("Tên quận/huyện")]
        public required string DistrictName { get; set; }
        [SwaggerSchema("Tên phường/xã/thị trấn")]
        public required string WardName { get; set; }
        [SwaggerSchema("Địa chỉ nhà?")]
        public bool IsHome { get; set; }
        [SwaggerSchema("Địa chỉ mặc định?")]
        public bool IsDefault { get; set; }
    }
}