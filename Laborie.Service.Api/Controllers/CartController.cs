using Laborie.Service.Application.Command.Cart;
using Laborie.Service.Application.Queries.Cart;
using Laborie.Service.Shared.Constant;
using Laborie.Service.Shared.Extensions;
using Laborie.Service.Shared.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Laborie.Service.Api.Controllers;

/// <summary>
/// Giỏ hàng
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/cart")]
[ApiVersion("1.0")]
public class CartController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Lấy thông tin giỏ hàng
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCart()
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = token.GetDataFromToken(AppConstants.AuthorizationSub);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var response = await sender.Send(new GetCartQuery(userId));
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Thêm sản phẩm vào giỏ hàng
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCart([FromBody] AddCartRequest input)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = token.GetDataFromToken(AppConstants.AuthorizationSub);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var command = input.Adapt<AddCart>() with
        {
            UserId = userId
        };
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }
}
