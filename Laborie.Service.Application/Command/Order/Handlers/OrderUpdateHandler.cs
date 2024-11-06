using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Command.Order.Handlers;


public class OrderUpdateHandler(ILogger<OrderUpdateHandler> logger) : ICommandHandler<OrderUpdate, Response>
{
    public async Task<Response> Handle(OrderUpdate request, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: code

            return new Response(StatusCodes.Status200OK, "Success", "");
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
        }
    }
}

