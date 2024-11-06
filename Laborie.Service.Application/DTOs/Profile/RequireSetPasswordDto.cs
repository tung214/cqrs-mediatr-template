namespace Laborie.Service.Application.DTOs.Profile
{
    public class RequireSetPasswordDto
    {
        public required string UserId { get; set; }
        public bool RequireSetPassword { get; set; }
    }
}