using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using it_explained.WebApi.Domain.Models;
using AutoMapper;

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

        try
        {
            // Force a connection by listing databases (triggers server interaction)
            var databases = MongoClient.ListDatabaseNames().ToList();

            Console.WriteLine("Connected to MongoDB. Databases:");
            foreach (var db in databases)
            {
                Console.WriteLine($" - {db}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MongoDB connection failed: {ex.Message}");
            Environment.Exit(1); // Optional: stop app if Mongo isn't reachable
        }
    }
}