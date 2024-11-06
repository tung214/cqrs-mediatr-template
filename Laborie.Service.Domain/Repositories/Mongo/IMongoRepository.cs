using System.Linq.Expressions;
using Laborie.Service.Domain.Entities.Mongo;
using MongoDB.Driver;

namespace Laborie.Service.Domain.Repositories.Mongo;
public interface IMongoRepository<TDocument> where TDocument : IMongoDocument
{
    TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);
    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);
    Task<List<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filterExpression);
    TDocument FindById(string id);
    void InsertOne(TDocument document);
    Task InsertOneAsync(TDocument document);
    void InsertMany(ICollection<TDocument> documents);
    Task InsertManyAsync(ICollection<TDocument> documents);
    Task<UpdateResult> Update(Dictionary<string, object?> listUpdate, string id);
    Task BulkUpdate(List<WriteModel<TDocument>> listWrites);
}
