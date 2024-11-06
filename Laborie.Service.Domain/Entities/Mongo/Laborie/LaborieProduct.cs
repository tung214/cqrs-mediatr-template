using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laborie.Service.Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laborie.Service.Domain.Entities.Mongo.Laborie
{
    [BsonCollection("Laborie", "LaborieProduct")]
    public class LaborieProduct : MongoDocument
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<LaborieProductImage>? Images { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOutOfStock { get; set; } = false;

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string>? Variants { get; set; }
        /// <summary>
        /// Có phải là gói đại lý không
        /// </summary>
        /// <value></value>
        public bool? IsPackage { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        #region Thông tin Shop
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Category { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? SubCategory { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Brand { get; set; }
        #endregion
    }
}