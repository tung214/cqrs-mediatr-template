using Newtonsoft.Json;

namespace Laborie.Service.Application.Command.Login;

/// <summary>
/// 
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
/// <param name="DeviceToken">token thiết bị</param>
public sealed record LoginRequest(string Email, string Password, string DeviceToken);

public sealed record RegisterRequest(string? Name, string Email, string? ReferCode);

public sealed record ResendActiveCodeRequest(string Email);

public sealed record ActiveRequest(string Email, string ActiveCode);

public sealed record ResetPasswordRequest(string Email);

public sealed record ChangePasswordRequest(string OldPassword, string NewPassword);

public sealed record SetPasswordRequest(string NewPassword, string DeviceToken);