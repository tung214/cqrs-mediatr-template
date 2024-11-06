using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laborie.Service.Domain.Entities.Mongo.Shop
{
    [BsonCollection("Shop", "fulfillment_wards")]
    public class FulfillmentWards : MongoDocument
    {
        [BsonElement("name")]
        public required string Name { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("provinceId")]
        public required string ProvinceId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("districtId")]
        public required string DistrictId { get; set; }
    }
}