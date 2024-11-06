using Laborie.Service.Application.Command.Address;
using Laborie.Service.Application.Queries.Address;
using Laborie.Service.Shared.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Laborie.Service.Api.Controllers;

/// <summary>
/// Thông tin địa chỉ đặt hàng user
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/address")]
[ApiVersion("1.0")]
public class AddressController(ISender sender) : BaseController
{
    /// <summary>
    /// Lấy ds địa chỉ đặt hàng
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get()
    {
        var userId = GetUserFromToken();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var response = await sender.Send(new GetAddressQuery(userId));
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Thêm địa chỉ đặt hàng
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Add([FromBody] AddressAddRequest input)
    {
        var userId = GetUserFromToken();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var command = input.Adapt<AddressAdd>() with
        {
            UserId = userId
        };

        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Thêm địa chỉ đặt hàng
    /// </summary>
    /// <returns></returns>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(string id, [FromBody] AddressUpdateRequest input)
    {
        var userId = GetUserFromToken();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var command = input.Adapt<AddressUpdate>() with
        {
            UserId = userId,
            AddressId = id
        };

        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Thêm địa chỉ đặt hàng
    /// </summary>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(Response), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = GetUserFromToken();
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var command = new AddressDelete(UserId: userId, AddressId: id);
        var response = await sender.Send(command);
        return StatusCode(response.StatusCode, response);
    }
}
