using Laborie.Service.Application.Command.Profile;
using Laborie.Service.Application.Queries.Profile;
using Laborie.Service.Shared.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Laborie.Service.Api.Controllers;


/// <summary>
/// Profile người dùng
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/profile")]
[ApiVersion("1.0")]
public class ProfileController(ISender sender) : BaseController
{

    /// <summary>
    /// Lấy thông tin profile
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [SwaggerResponse(StatusCodes.Status200OK, "Thông tin tài khoản", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Tài khoản không tồn tại", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Token không hợp lệ, cần login lại", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Invalid Request", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetUserFromToken();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var response = await sender.Send(new GetProfileQuery(userId));
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Cập nhật profile
    /// </summary>
    /// <returns></returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Token không hợp lệ, cần login lại", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Tài khoản không tồn tại", typeof(Response))]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Invalid Request", typeof(Response))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateRequest input)
    {
        var userId = GetUserFromToken();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var command = input.Adapt<ProfileUpdate>() with
        {
            UserId = userId,
        };
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }
}
