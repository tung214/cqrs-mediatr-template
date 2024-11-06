using Laborie.Service.Application.DTOs.Cart;
using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Queries.Cart.Handlers;
public class GetCartQueryHandler(ILogger<GetCartQueryHandler> logger) : IQueryHandler<GetCartQuery, Response>
{
    public async Task<Response> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: get cart



            var data = new List<CartItemDto> {
                    new() {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Nº.5 BOND MAINTENANCE® CONDITIONER",
                        Image = "https://static.30shine.com/shop-admin/2023/03/06/30SXU6JL-COMBO%208.jpg",
                        VersionName = "100ml",
                        Price = 229000,
                        Quantity = 1
                    },
                    new() {
                        Id = Guid.NewGuid().ToString(),
                        Name = "Combo Hồi Xuân Trai Trẻ",
                        Image = "https://static.30shine.com/shop-admin/2023/03/06/30SXU6JL-COMBO%208.jpg",
                        VersionName = "",
                        Price = 867000,
                        Quantity = 1
                    },
                };
            return new Response<List<CartItemDto>>(StatusCodes.Status200OK, "Success", "") with
            {
                Data = data
            };
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
        }
    }
}
