using Laborie.Service.Application.DTOs.Order;
using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Queries.Orders.Handlers;

public class GetPendingOrderQueryHandler(ILogger<GetPendingOrderQueryHandler> logger) : IQueryHandler<GetPendingOrderQuery, Response>
{
    public async Task<Response> Handle(GetPendingOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: code



            var data = new List<OrderListItemDto> {
                    new() {
                        Id = Guid.NewGuid().ToString(),
                        Status = "Chờ xác nhận",
                        IsConfirm = true,
                        Name = "Máy sấy tóc Furin - Mạnh gấp 10 máy sấy bạn có",
                        TotalPrice = 389000,
                        NumberItems = 3
                    },
                    new() {
                        Id = Guid.NewGuid().ToString(),
                        Status = "Đang giao hàng",
                        IsShipping = true,
                        Name = "Lăn Khử Mùi Cool Men ULTRASENSITIVE Dành Cho Da Nhạy cảm",
                        TotalPrice = 900000,
                        NumberItems = 1
                    },
                };
            return new ResponsePaging<OrderListItemDto>(StatusCodes.Status200OK, "Success", "") with
            {
                Total = 2,
                TotalPage = 1,
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
