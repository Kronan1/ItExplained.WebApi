using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using it_explained.WebApi.Domain.Models;
using AutoMapper;

namespace it_explained.WebApi.Repository.DbContext;

public class MongoDbContextService
{
    private IMongoDatabase _database;

    public MongoDbContextService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("knowledge_core");
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}