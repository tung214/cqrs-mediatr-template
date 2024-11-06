using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Laborie.Service.Application.Command.Address.Handlers;
public class AddressAddHandler(ILogger<AddressAddHandler> logger
    // , IMongoRepository<LaborieAgency> agencyRepository
    , IMongoRepository<LaborieAgencyAddress> agencyAddressRepository
    , IAgencyAddressRepository addressRepository)
: ICommandHandler<AddressAdd, Response>
{
    public async Task<Response> Handle(AddressAdd request, CancellationToken cancellationToken)
    {
        try
        {
            var address = await addressRepository.GetAddress(request.ProvinceId, request.DistrictId, request.WardId);
            if (address == null || address.Province == null || address.District == null || address.Ward == null)
                return new Response(StatusCodes.Status400BadRequest, "Không tìm thấy thông tin địa chỉ", "Address not found Fulfillment");

            // var agency = await agencyRepository.FindOneAsync(x => x.Id == request.UserId && x.IsDelete == false);
            var agency = await addressRepository.GetAgencyAddress(request.UserId);
            if (agency == null) return new Response(StatusCodes.Status400BadRequest, "Không tìm thấy thông tin đại lý", "User not found");

            var newAddress = new LaborieAgencyAddress
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address,
                Agency = request.UserId,
                ProvinceId = address.Province.Id,
                ProvinceName = address.Province.Name,
                DistrictId = address.District.Id,
                DistrictName = address.District.Name,
                WardId = address.Ward.Id,
                WardName = address.Ward.Name,
                IsHome = request.IsHome,
                IsDefault = request.IsDefault,
                CreatedDate = DateTime.Now
            };

            // Current address empty -> new address set default
            if (agency.Addresses == null || agency.Addresses.Count == 0)
            {
                newAddress.IsDefault = true;
            }
            else if (request.IsDefault)
            {
                // update old address default -> not
                var addressIsDefault = agency.Addresses.FirstOrDefault(x => x.IsDefault);
                if (addressIsDefault != null)
                {
                    await agencyAddressRepository.Update(new Dictionary<string, object?> {
                        {nameof(addressIsDefault.IsDefault), false}
                    }, addressIsDefault.Id);
                }
            }

            await agencyAddressRepository.InsertOneAsync(newAddress);

            return new Response(StatusCodes.Status200OK, "Success", "");
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
        }
    }
}
