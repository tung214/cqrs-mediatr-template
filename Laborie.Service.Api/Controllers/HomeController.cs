using Laborie.Service.Application.DTOs.Home;
using Laborie.Service.Application.Queries.Home;
using Laborie.Service.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Laborie.Service.Api.Controllers;

/// <summary>
/// Home
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/home")]
[ApiVersion("1.0")]
public class HomeController(ISender sender) : ControllerBase
{

    /// <summary>
    /// Xem danh sách Banner
    /// </summary>
    /// <returns></returns>
    [HttpGet("banner")]
    [ProducesResponseType(typeof(Response<List<BannerDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Banner()
    {
        var response = await sender.Send(new GetBannerQuery());
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Xem danh sách sản phẩm
    /// </summary>
    /// <returns></returns>
    [HttpGet("product")]
    [ProducesResponseType(typeof(Response<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProducts()
    {
        var response = await sender.Send(new GetProductQuery());
        return StatusCode(response.StatusCode, response);
    }

    /// <summary>
    /// Xem chi tiết sản phẩm
    /// </summary>
    /// <returns></returns>
    [HttpGet("product/{id}/{variantId}")]
    [ProducesResponseType(typeof(Response<ProductItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Response), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProducts(string id, string variantId)
    {
        var response = await sender.Send(new GetProductItemQuery(id, variantId));
        return StatusCode(response.StatusCode, response);
    }
}
