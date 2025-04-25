using AutoMapper;
using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Repository.Initializers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace it_explained.WebApi.EndPoints;

public static class MongoDbSampleEndPoint
{
    public static IEndpointRouteBuilder MapMongoDbSample(this IEndpointRouteBuilder app, DbContextService dbContextService)
    {
        app.MapGet("/api/comments", () =>
        {
            try
            {
                var result = dbContextService.MongoClient.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            var dbName = "sample_mflix";
            var collectionName = "comments";

            var collection = dbContextService.MongoClient.GetDatabase(dbName)
               .GetCollection<Comment>(collectionName);

            var comments = collection.Find(Builders<Comment>.Filter.Empty)
              .ToList();

            return comments.FirstOrDefault();
        })
        .WithName("GetComments")
        .WithOpenApi(); // Ensure the endpoint is included in Swagger

        Console.WriteLine("after mapping");
        return app;
    }
}