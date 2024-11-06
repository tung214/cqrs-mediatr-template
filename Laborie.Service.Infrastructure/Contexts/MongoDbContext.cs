using MongoDB.Driver;

namespace Laborie.Service.Infrastructure.Contexts;
public class MongoDbContext
{
    public Dictionary<string, IMongoDatabase> Databases { get; set; } = [];
}
