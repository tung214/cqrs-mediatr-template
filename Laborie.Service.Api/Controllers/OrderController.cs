using Laborie.Service.Application.Command.Order;
using Laborie.Service.Application.Queries.Orders;
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
/// Đơn hàng
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/order")]
[ApiVersion("1.0")]
public class OrderController(ISender sender) : ControllerBase
{

    /// <summary>
    /// Lấy chi tiết đơn hàng
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrder([FromQuery] int pageSize, [FromQuery] int pageIndex)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = token.GetDataFromToken(AppConstants.AuthorizationSub);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var response = await sender.Send(new GetPendingOrderQuery(userId, pageSize, pageIndex));
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Lấy chi tiết đơn hàng
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrder(string id)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = token.GetDataFromToken(AppConstants.AuthorizationSub);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var response = await sender.Send(new GetOrderQuery(userId, id));
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Lấy lịch sử đơn hàng
    /// </summary>
    /// <returns></returns>
    [HttpGet("histories")]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetOrderHistory([FromQuery] int pageSize, [FromQuery] int pageIndex)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = token.GetDataFromToken(AppConstants.AuthorizationSub);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var response = await sender.Send(new GetOrderHistoryQuery(userId, pageSize, pageIndex));
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Tạo đơn hàng
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddOrder([FromBody] OrderRequest input)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = token.GetDataFromToken(AppConstants.AuthorizationSub);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var command = input.Adapt<OrderAdd>() with
        {
            UserId = userId
        };
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Cập nhật đơn hàng
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateOrder(string id, [FromBody] OrderRequest input)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = token.GetDataFromToken(AppConstants.AuthorizationSub);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var command = input.Adapt<OrderUpdate>() with
        {
            OrderId = id,
            UserId = userId
        };
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Hủy đơn hàng
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteOrder(string id)
    {
        var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
        var userId = token.GetDataFromToken(AppConstants.AuthorizationSub);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var command = new OrderDelete(UserId: userId, OrderId: id);
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }
}
