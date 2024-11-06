using Laborie.Service.Domain.Entities.Mongo;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Entities.Mongo.Shop;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Domain.ValueObjects;
using Laborie.Service.Infrastructure.Contexts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Laborie.Service.Infrastructure.Repositories.Mongo
{
    public class AgencyAddressRepository(ILogger<AgencyAddressRepository> logger
        , IOptions<MongoDbContext> options) : IAgencyAddressRepository
    {

        #region Laborie
#pragma warning disable CS8602, CS8620 // Dereference of a possibly null reference.
        private readonly IMongoCollection<LaborieAgencyAddress> agencyAddressCollection
            = options.Value.Databases.GetValueOrDefault(MongoExtension.GetDatabaseName(typeof(LaborieAgencyAddress)))
                .GetCollection<LaborieAgencyAddress>(MongoExtension.GetCollectionName(typeof(LaborieAgencyAddress)));
        private readonly IMongoCollection<LaborieAgency> agencyCollection
            = options.Value.Databases.GetValueOrDefault(MongoExtension.GetDatabaseName(typeof(LaborieAgency)))
                .GetCollection<LaborieAgency>(MongoExtension.GetCollectionName(typeof(LaborieAgency)));
#pragma warning restore CS8602, CS8620 // Dereference of a possibly null reference.
        #endregion Laborie

        #region Shop
#pragma warning disable CS8602, CS8620 // Dereference of a possibly null reference.
        private readonly IMongoCollection<FulfillmentProvinces> fulfillmentProvincesCollection
           = options.Value.Databases.GetValueOrDefault(MongoExtension.GetDatabaseName(typeof(FulfillmentProvinces)))
               .GetCollection<FulfillmentProvinces>(MongoExtension.GetCollectionName(typeof(FulfillmentProvinces)));

        private readonly IMongoCollection<FulfillmentDistricts> fulfillmentDistrictsCollection
            = options.Value.Databases.GetValueOrDefault(MongoExtension.GetDatabaseName(typeof(FulfillmentDistricts)))
                .GetCollection<FulfillmentDistricts>(MongoExtension.GetCollectionName(typeof(FulfillmentDistricts)));

        private readonly IMongoCollection<FulfillmentWards> fulfillmentWardsCollection
            = options.Value.Databases.GetValueOrDefault(MongoExtension.GetDatabaseName(typeof(FulfillmentWards)))
                .GetCollection<FulfillmentWards>(MongoExtension.GetCollectionName(typeof(FulfillmentWards)));
#pragma warning restore CS8602, CS8620 // Dereference of a possibly null reference.
        #endregion Shop

        public async Task<AgencyAddressVO?> GetAgencyAddress(string agencyId)
        {
            try
            {
                var joinedData = await agencyCollection.Aggregate()
                        .Match(x => x.Id == agencyId && x.IsDelete == false)
                        .Lookup<LaborieAgency, LaborieAgencyAddress, AgencyAddressVO>(
                            agencyAddressCollection,
                            local => local.Id,             // Local field
                            foreign => foreign.Agency,       // Foreign field
                            result => result.Addresses // Target property
                        )
                        .Match(x => x.Addresses == null || x.Addresses.Any(o => o.IsDelete == false))
                        .Project(item => new AgencyAddressVO
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Salt = "",
                            Email = "",
                            Addresses = item.Addresses == null ? new List<LaborieAgencyAddress>()
                                        : item.Addresses.Where(o => o.IsDelete == false).ToList() // Điều kiện lọc Orders có IsComplete = true
                        }
                        )
                        .FirstOrDefaultAsync();

                // if (joinedData.Addresses != null && joinedData.Addresses.Count > 0)
                // {
                //     joinedData.Addresses = joinedData.Addresses.Where(x => x.IsDelete == false).ToList();
                // }
                return joinedData;
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "{classs} has error {error}", GetType().Name, ex.Message);
                return null;
            }
        }

        public async Task<AddressVO?> GetAddress(string provinceId, string districtId, string wardId)
        {
            try
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var data = await fulfillmentWardsCollection.Aggregate()
                    // Match ward with specific Id
                    .Match(ward => ward.Id == wardId && ward.DistrictId == districtId && ward.ProvinceId == provinceId)
                    .Project(w => new AddressVO
                    {
                        Ward = w // Gán dữ liệu Ward vào AddressVO
                    })
                    // Join with Districts based on districtId
                    .Lookup<AddressVO, FulfillmentDistricts, AddressVO>(
                        fulfillmentDistrictsCollection,
                        local => local.Ward.DistrictId,         // Local field in AddressVO
                        foreign => foreign.Id,          // Foreign field in FulfillmentDistricts
                        result => result.District         // Target property in AddressVO
                    )
                    .Unwind<AddressVO, AddressVO>(result => result.District) // Bung kết quả của District ra
                    .Lookup<AddressVO, FulfillmentProvinces, AddressVO>(
                        fulfillmentProvincesCollection,
                        local => local.District.ProvinceId, // Local field in Districts
                        foreign => foreign.Id,                  // Foreign field in Provinces
                        result => result.Province   // result in Province
                    )
                    .Unwind<AddressVO, AddressVO>(result => result.Province) // Bung kết quả của Province ra
                    .FirstOrDefaultAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                return data;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{classs} has error {error}", GetType().Name, ex.Message);
                return null;
            }
        }
    }
}