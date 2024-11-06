using Laborie.Service.Application.DTOs.Address;
using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Queries.Address.Handlers;
public class GetAddressQueryHandler(ILogger<GetAddressQueryHandler> logger
    , IAgencyAddressRepository agencyAddressRepository
)
: IQueryHandler<GetAddressQuery, Response>
{
    public async Task<Response> Handle(GetAddressQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var address = await agencyAddressRepository.GetAgencyAddress(request.UserId);
            var data = address?.Addresses?.Select(x => x.Adapt<AddressItemDto>()).ToList();
        
            return new Response<List<AddressItemDto>>(StatusCodes.Status200OK, "Success", "") with
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
