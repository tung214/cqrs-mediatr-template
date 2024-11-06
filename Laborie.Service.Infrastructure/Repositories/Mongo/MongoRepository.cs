using System.Linq.Expressions;
using Laborie.Service.Domain.Entities.Mongo;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Infrastructure.Contexts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Laborie.Service.Infrastructure.Repositories.Mongo;
public class MongoRepository<TDocument>(IOptions<MongoDbContext> options) : IMongoRepository<TDocument>
where TDocument : IMongoDocument
{
    private readonly IMongoCollection<TDocument> _collection =
#pragma warning disable CS8602,CS8620 // Dereference of a possibly null reference.
        options.Value.Databases.GetValueOrDefault(MongoExtension.GetDatabaseName(typeof(TDocument)))
#pragma warning restore CS8602,CS8620 // Dereference of a possibly null reference.
            .GetCollection<TDocument>(MongoExtension.GetCollectionName(typeof(TDocument)));

    public TDocument FindById(string id)
    {
        return _collection.Find(s => s.Id == id).FirstOrDefault();
    }

    public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).FirstOrDefault();
    }

    public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).FirstOrDefaultAsync();
    }

    public void InsertMany(ICollection<TDocument> documents)
    {
        _collection.InsertMany(documents);
    }

    public async Task InsertManyAsync(ICollection<TDocument> documents)
    {
        await _collection.InsertManyAsync(documents);
    }

    public void InsertOne(TDocument document)
    {
        _collection.InsertOne(document);
    }

    public async Task InsertOneAsync(TDocument document)
    {
        await _collection.InsertOneAsync(document);
    }
    public async Task<List<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).ToListAsync();
    }

    public async Task<UpdateResult> Update(Dictionary<string, object?> listUpdate, string id)
    {
        var update = Builders<TDocument>.Update;
        var updates = new List<UpdateDefinition<TDocument>>();

        foreach (var item in listUpdate)
        {
            updates.Add(update.Set(item.Key, item.Value));
        }
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
        return await _collection.UpdateOneAsync(filter, update.Combine(updates));
    }

    public async Task BulkUpdate(List<WriteModel<TDocument>> listWrites)
    {
        await _collection.BulkWriteAsync(listWrites);
    }
}
