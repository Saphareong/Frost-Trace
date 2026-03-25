using FrostTrace.Api.Domain.Entities;
using MongoDB.Driver;

namespace FrostTrace.Api.Infrastructure.Persistence;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IMongoClient client, IConfiguration configuration)
    {
        var dbName = configuration["MongoDB:DatabaseName"] ?? "FrostTraceDb";
        _database = client.GetDatabase(dbName);
    }

    public IMongoCollection<Batch> Batches => _database.GetCollection<Batch>("Batches");
    public IMongoCollection<Handshake> Handshakes => _database.GetCollection<Handshake>("Handshakes");
}
