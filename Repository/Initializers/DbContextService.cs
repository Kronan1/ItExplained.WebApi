using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace it_explained.WebApi.Repository.Initializers;

public class DbContextService
{
    public MongoClient MongoClient { get; set; }

    public DbContextService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDb");
        if (string.IsNullOrEmpty(connectionString))
        {
            Environment.Exit(0);
        }
        MongoClient = new MongoClient(connectionString);
    }
}