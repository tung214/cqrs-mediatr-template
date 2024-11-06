using Laborie.Service.Domain.Attributes;

namespace Laborie.Service.Domain.Entities.Mongo;

public static class MongoExtension
{
    public static string? GetDatabaseName(this Type documentType)
    {
#pragma warning disable CS8600,CS8603 // Converting null literal or possible null value to non-nullable type.
        return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true).FirstOrDefault())?.DatabaseName;
#pragma warning restore CS8600,CS8603 // Converting null literal or possible null value to non-nullable type.
    }

    public static string GetCollectionName(this Type documentType)
    {
#pragma warning disable CS8600,CS8603 // Converting null literal or possible null value to non-nullable type.
        return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true).FirstOrDefault())?.CollectionName;
#pragma warning restore CS8600,CS8603 // Converting null literal or possible null value to non-nullable type.
    }
}