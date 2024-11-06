using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Command.Address.Handlers;

public class AddressDeleteHandler(ILogger<AddressDeleteHandler> logger
    , IMongoRepository<LaborieAgencyAddress> agencyAddressRepository
    , IAgencyAddressRepository addressRepository
)
: ICommandHandler<AddressDelete, Response>
{
    public async Task<Response> Handle(AddressDelete request, CancellationToken cancellationToken)
    {
        try
        {
            var agency = await addressRepository.GetAgencyAddress(request.UserId);
            if (agency == null) return new Response(StatusCodes.Status400BadRequest, "Không tìm thấy thông tin đại lý", "User not found");

            var current = agency.Addresses?.FirstOrDefault(x => x.Id == request.AddressId);
            if (current == null) return new Response(StatusCodes.Status400BadRequest, "Không tìm thấy thông tin địa chỉ", "Address not found");

            await agencyAddressRepository.Update(new Dictionary<string, object?> {
                {nameof(current.IsDelete), true}
            }, current.Id);

            return new Response(StatusCodes.Status200OK, "Success", "");
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
        }
    }
}

