using Laborie.Service.Application.DTOs.Order;
using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Queries.Orders.Handlers;
public class GetOrderQueryHandler(ILogger<GetOrderQueryHandler> logger) : IQueryHandler<GetOrderQuery, Response>
{
    public async Task<Response> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: code
            var data = new OrderDto
            {
                Id = Guid.NewGuid().ToString(),
                IsConfirm = true,
                Status = "Chờ xác nhận",
                TotalPrice = 741000,
                OrderItems = new List<OrderItemDto>
                {
                    new() {
                        ProductId = Guid.NewGuid().ToString(),
                        ProductName = "Sáp Reuzel Red Pomade Giữ Nếp Vừa - Độ Bóng Cao - Gốc Nước",
                        Price = 382000,
                        Quantity = 1
                    },
                    new() {
                        ProductId = Guid.NewGuid().ToString(),
                        ProductName = "Combo Tóc Đẹp Máy Sấy Tóc Sharkway + Tinh Dầu DR. MACADAMIA",
                        Price = 359000,
                        Quantity = 1
                    }
                }
            };
            return new Response<OrderDto>(StatusCodes.Status200OK, "Success", "") with
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
