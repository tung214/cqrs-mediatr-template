using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.Attributes;
using MongoDB.Bson.Serialization.Attributes;

namespace Laborie.Service.Domain.Entities.Mongo.Shop
{
    [BsonCollection("Shop", "fulfillment_provinces")]
    public class FulfillmentProvinces : MongoDocument
    {
        [BsonElement("name")]
        public required string Name { get; set; }
    }
}