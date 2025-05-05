using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Repository.DbContext;
using MongoDB.Driver;

namespace it_explained.WebApi.EndPoints;
public static class TopicEndPoint
{
    public static IEndpointRouteBuilder MapTopic(this IEndpointRouteBuilder app, MongoDbContextService dbContextService, IConfiguration configuration)
    {
        var client = new MongoClient();
        var db = client.GetDatabase("knowledge_core");
        var collection = db.GetCollection<Topic>("topics");

        app.MapPost("/topic/get-topics", async (HttpContext context, string requestedTopic) =>
        {
            try
            {
                var filter = Builders<Topic>.Filter.Eq("name", requestedTopic);
                var topic = await collection.Find(filter).FirstOrDefaultAsync();

                return topic != null ? Results.Ok(topic) : Results.NotFound("No topic found with name " + requestedTopic);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
        })
        .WithName("GetTopics")
        .WithOpenApi();

            return app;
    }
}
