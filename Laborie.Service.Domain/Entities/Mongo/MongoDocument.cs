using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Laborie.Service.Domain.Entities.Mongo;
public class MongoDocument : IMongoDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
#pragma warning disable CS8618 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
    public string Id { get; set; }
#pragma warning restore CS8618 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
}
