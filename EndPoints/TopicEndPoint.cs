using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Repository.DbContext;
using MongoDB.Driver;

namespace it_explained.WebApi.EndPoints;
public static class TopicEndPoint
{
    public static IEndpointRouteBuilder MapTopic(this IEndpointRouteBuilder app)
    {
        app.MapPost("/topic/get-test-topic", async (IMongoClient mongoClient) =>
        {
            try
            {
                var db = mongoClient.GetDatabase("knowledge_core");
                var collection = db.GetCollection<Topic>("topics");

                var filter = Builders<Topic>.Filter.Eq(x => x.Name, "TLS");
                var topic = await collection.Find(filter).FirstOrDefaultAsync();

                return topic != null ? Results.Ok(topic) : Results.NotFound("test topic failed");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
        .WithName("GetFirstTopic")
        .WithOpenApi();

        app.MapPost("/topic/get-all", async (IMongoClient mongoClient) =>
        {
            try
            {
                var db = mongoClient.GetDatabase("knowledge_core");
                var collection = db.GetCollection<Topic>("topics");

                var filter = Builders<Topic>.Filter.Empty;
                var topics = await collection.Find(filter).ToListAsync();

                return topics != null ? Results.Ok(topics) : Results.NotFound("Could not get collection");
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        })
      .WithName("GetAll")
      .WithOpenApi();

        return app;
    }
}
