using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Application.DTOs.Profile;

public class ProfileItemDto
{
    public string? Id { get; set; }
    [SwaggerSchema("Tên đại lý")]
    public string? Name { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    [SwaggerSchema("Mã giới thiệu, dùng để giới thiệu đại lý mới")]
    public string? ReferCode { get; set; }
    [SwaggerSchema("% hoa hồng được hưởng")]
    public float? ReferCommission { get; set; }
}
