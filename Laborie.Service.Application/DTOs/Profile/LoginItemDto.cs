namespace Laborie.Service.Application.DTOs.Profile;

public class LoginItemDto
{
    public string AccessToken { get; set; } = "";
    public ProfileItemDto? Profile { get; set; }
}
