using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Repository.Initializers;
using MongoDB.Driver;

namespace it_explained.WebApi.EndPoints;
public static class TopicEndPoint
{
    public static IEndpointRouteBuilder MapTopic(this IEndpointRouteBuilder app, DbContextService dbContextService, IConfiguration configuration)
    {
        var client = dbContextService.MongoClient;
        var db = client.GetDatabase("knowledge_core");
        var collection = db.GetCollection<Topic>("topics");

        app.MapPost("/api/add-topic", async (HttpContext context, string requestedTopic) =>
        {
            try
            {
                // Add logic for getting topic from HD

                string data = File.ReadAllText("Mock/MiscTopic.md");

                Topic topic = Helpers.MarkdownParser.ParseTopic(data);

                await collection.InsertOneAsync(topic);

                return Results.Created($"/api/add-topic/{topic.Id}", new { message = "Topic added to the database", topic });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
            
        })
        .WithName("AddTopic")
        .WithOpenApi();

        app.MapPost("/api/add-topics", async (HttpContext context) =>
        {
            var folderPath = configuration["Configuration:FolderPath"];

            if (folderPath == null)
                return Results.Problem($"Configured folder path '{folderPath}' does not exist.");

            // Get all files
            var files = Directory.GetFiles(folderPath);
            List<Topic> completedCollection = [];
            List<string> errorCollection = [];

            // Now files is a string[] of full file paths
            foreach (var file in files)
            {
                string content = await File.ReadAllTextAsync(file);
                Topic topic = Helpers.MarkdownParser.ParseTopic(content);
                if (topic != null)
                {
                    completedCollection.Add(topic);
                }
                else
                {
                    errorCollection.Add(file);
                }
            }

            if (completedCollection.Count > 0)
                await collection.InsertManyAsync(completedCollection);

            return errorCollection.Count == 0 ? 
                Results.Ok($"All {completedCollection.Count} Completed") : 
                    Results.Problem($"{completedCollection.Count} Successfully completed.\nFailed {errorCollection}");

        })
        .WithName("AddTopics")
        .WithOpenApi();


        app.MapPost("/api/get-topic", async (HttpContext context, string requestedTopic) =>
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
        .WithName("GetTopic")
        .WithOpenApi();

            return app;
    }
}
