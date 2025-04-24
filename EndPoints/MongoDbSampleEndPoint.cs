using AutoMapper;
using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Repository.Initializers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace it_explained.WebApi.EndPoints;

public static class MongoDbSampleEndPoint
{
    public static IEndpointRouteBuilder MapMongoDbSample(this IEndpointRouteBuilder app, DbContextService dbContextService, IMapper mapper)
    {
        app.MapGet("/api/comments", (HttpContext context) =>
        {
            //var collection = dbContextService.MongoClient
            //    .GetDatabase("sample_mflix")
            //    .GetCollection<Comment>("comments");

            //var data = collection.Find(Builders<Comment>.Filter.Empty).ToListAsync();

            //data.Wait();

            //var result = mapper.Map<IEnumerable<Comment>>(data);
            //context.Response.WriteAsJsonAsync(result);

            //Console.WriteLine("end of mapping");

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