using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Laborie.Service.Application.Command.Address.Handlers;


public class AddressUpdateHandler(ILogger<AddressUpdateHandler> logger
    , IMongoRepository<LaborieAgencyAddress> agencyAddressRepository
    , IAgencyAddressRepository addressRepository
)
: ICommandHandler<AddressUpdate, Response>
{
    public async Task<Response> Handle(AddressUpdate request, CancellationToken cancellationToken)
    {
        try
        {
            var address = await addressRepository.GetAddress(request.ProvinceId, request.DistrictId, request.WardId);
            if (address == null || address.Province == null || address.District == null || address.Ward == null)
                return new Response(StatusCodes.Status400BadRequest, "Không tìm thấy thông tin địa chỉ", "Address not found Fulfillment");

            var agency = await addressRepository.GetAgencyAddress(request.UserId);
            if (agency == null) return new Response(StatusCodes.Status400BadRequest, "Không tìm thấy thông tin đại lý", "User not found");

            var current = agency.Addresses?.FirstOrDefault(x => x.Id == request.AddressId);
            if (current == null) return new Response(StatusCodes.Status400BadRequest, "Không tìm thấy thông tin địa chỉ", "Address not found");


            var listWriteUpdate = new List<WriteModel<LaborieAgencyAddress>>();

            #region Update current
            var filterDefinition = Builders<LaborieAgencyAddress>.Filter.Eq(p => p.Id, current.Id);
            var updateDefinition = new List<UpdateDefinition<LaborieAgencyAddress>>
                    {
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.Name, request.Name),
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.IsHome, request.IsHome),
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.IsDefault, request.IsDefault),
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.Address, request.Address),
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.ProvinceId, address.Province.Id),
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.ProvinceName, address.Province.Name),
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.DistrictId, address.District.Id),
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.DistrictName, address.District.Name),
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.WardId, address.Ward.Id),
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.WardName, address.Ward.Name),
                    };

            listWriteUpdate.Add(new UpdateOneModel<LaborieAgencyAddress>(filterDefinition,
                Builders<LaborieAgencyAddress>.Update.Combine(updateDefinition)));
            #endregion

            #region Update not default for old address
            if (agency.Addresses != null && agency.Addresses.Count > 2 && request.IsDefault)
            {
                // update old address default -> not
                var addressIsDefault = agency.Addresses.FirstOrDefault(x => x.IsDefault && x.Id != current.Id);
                if (addressIsDefault != null)
                {
                    var filterDefinitionUpdate = Builders<LaborieAgencyAddress>.Filter.Eq(p => p.Id, addressIsDefault.Id);
                    var updateDefinitionUpdate = new List<UpdateDefinition<LaborieAgencyAddress>>
                    {
                        Builders<LaborieAgencyAddress>.Update.Set(x => x.IsDefault, false),
                    };

                    listWriteUpdate.Add(new UpdateOneModel<LaborieAgencyAddress>(filterDefinitionUpdate,
                        Builders<LaborieAgencyAddress>.Update.Combine(updateDefinitionUpdate)));
                }
            }
            #endregion Update not default for old address

            await agencyAddressRepository.BulkUpdate(listWriteUpdate);

            return new Response(StatusCodes.Status200OK, "Success", "");
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
        }
    }
}