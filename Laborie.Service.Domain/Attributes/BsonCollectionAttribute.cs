namespace Laborie.Service.Domain.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BsonCollectionAttribute(string databaseName, string collectionName) : Attribute
{
    public string CollectionName { get; set; } = collectionName;
    public string DatabaseName { get; set; } = databaseName;
}
