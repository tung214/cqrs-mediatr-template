using Laborie.Service.Domain.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laborie.Service.Domain.Entities.Mongo.Laborie
{
    [BsonCollection("Laborie", "LaborieProductVariants")]
    public class LaborieProductVariants : MongoDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Product { get; set; }
        public required string Name { get; set; }
        public required string Sku { get; set; }
        public int Price { get; set; }
        public LaborieProductImage? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsOutOfStock { get; set; } = false;
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}