using Laborie.Service.Application.Command.Login;
using Laborie.Service.Application.DTOs.Profile;
using Laborie.Service.Shared.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Api.Controllers;

/// <summary>
/// Đăng nhập, tạo mới tài khoản...
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/authentication")]
[ApiVersion("1.0")]
public class AuthenticationController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Đăng ký tài khoản
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [SwaggerResponse(StatusCodes.Status201Created, "Tài khoản được tạo, chưa kích hoạt", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Invalid Request", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest input)
    {
        var command = input.Adapt<Register>();
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Kích hoạt tài khoản
    /// </summary>
    /// <param name="input">Active code</param>
    /// <returns></returns>
    [HttpPost("active")]
    [SwaggerResponse(StatusCodes.Status200OK, "Tài khoản được kích hoạt, cần thiết lập mật khẩu", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Invalid Request", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Active([FromBody] ActiveRequest input)
    {
        var command = input.Adapt<Active>();
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Gửi lại mail kích hoạt
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("active/resend")]
    [SwaggerResponse(StatusCodes.Status200OK, "Gửi mã kích hoạt thành công", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status205ResetContent, "Tài khoản đã kích hoạt, cần thiết lập mật khẩu", typeof(Response<RequireSetPasswordDto>))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Invalid Request", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResendActive([FromBody] ResendActiveCodeRequest input)
    {
        var command = input.Adapt<ResendActiveCode>();
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }


    /// <summary>
    /// Login
    /// </summary>
    /// <returns></returns>
    [HttpPost("login")]
    [SwaggerResponse(StatusCodes.Status200OK, "Đăng nhập, trả về Access token và thông tin tài khoản", typeof(Response<LoginItemDto>))]
    [SwaggerResponse(StatusCodes.Status406NotAcceptable, "Tài khoản hoặc mật khẩu không đúng", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Invalid Request", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest input)
    {
        var command = input.Adapt<Login>();
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Reset mật khẩu
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("reset-password")]
    [SwaggerResponse(StatusCodes.Status202Accepted, "Mật khẩu reset được gửi về email", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Tài khoản không đúng", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status406NotAcceptable, "Tài khoản chưa được kích hoạt, cần kích hoạt tài khoản", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Invalid Request", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest input)
    {
        var command = input.Adapt<ResetPassword>();
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Đặt mật khẩu
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("set-password/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, "Mật khẩu được thiết lập, trả về Access token và thông tin tài khoản", typeof(Response<LoginItemDto>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Tài khoản không đúng", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Invalid Request", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangePassword(string id, [FromBody] SetPasswordRequest input)
    {
        var command = input.Adapt<SetPassword>() with
        {
            UserId = id
        };
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Đổi mật khẩu
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("change-password/{id}")]
    [SwaggerResponse(StatusCodes.Status202Accepted, "Mật khẩu được cập nhật", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Tài khoản hoặc mật khẩu không đúng", typeof(Response))]    
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Invalid Request", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangePassword(string id, [FromBody] ChangePasswordRequest input)
    {
        var command = input.Adapt<ChangePassword>() with
        {
            UserId = id
        };
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }
}
