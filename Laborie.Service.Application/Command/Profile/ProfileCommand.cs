using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Command.Profile;


public sealed record ProfileUpdate(string UserId
    , string? Name
    , string? Phone
    , string? Email
    , DateTime? DateOfBirth
) : ICommand<Response>;