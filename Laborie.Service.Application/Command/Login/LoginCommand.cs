using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Command.Login;

public sealed record Login(string Email, string Password, string DeviceToken) : ICommand<Response>;

public sealed record Register(string Name, string Email, string? ReferCode) : ICommand<Response>;

public sealed record ResendActiveCode(string Email) : ICommand<Response>;

public sealed record Active(string Email, string ActiveCode) : ICommand<Response>;

public sealed record ResetPassword(string Email) : ICommand<Response>;

public sealed record ChangePassword(string UserId, string OldPassword, string NewPassword) : ICommand<Response>;

public sealed record SetPassword(string UserId, string NewPassword, string DeviceToken) : ICommand<Response>;