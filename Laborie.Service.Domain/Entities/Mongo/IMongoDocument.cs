using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laborie.Service.Domain.Entities.Mongo;
public interface IMongoDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }
}
