using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laborie.Service.Domain.Entities.Mongo.Laborie
{
    [BsonCollection("Laborie", "LaborieAgencyAddress")]
    public class LaborieAgencyAddress : MongoDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Agency { get; set; }
        public required string Name { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required string ProvinceName { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string ProvinceId { get; set; }
        public required string DistrictName { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string DistrictId { get; set; }
        public required string WardName { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string WardId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsHome { get; set; } = false;
        public bool IsDefault { get; set; } = false;
        public bool IsDelete { get; set; } = false;
    }
}