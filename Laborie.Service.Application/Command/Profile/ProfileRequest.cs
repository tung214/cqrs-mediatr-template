namespace Laborie.Service.Application.Command.Profile;

public sealed record ProfileUpdateRequest(string? Name
    , string? Phone
    , string? Email
    , DateTime? DateOfBirth
);