using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Repository.Initializers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace it_explained.WebApi.EndPoints;
public static class SaveDataEndPoint
{
    public static IEndpointRouteBuilder MapSaveTopic(this IEndpointRouteBuilder app, DbContextService dbContextService)
    {
        app.MapPost("/api/add-topic", async (HttpContext context, string requestedTopic) =>
        {
            try
            {
                // Add logic for getting topic from HD

                // Add logic for topic not found

                if (requestedTopic != "MiscTopic")
                    return Results.NotFound(new { message = requestedTopic + " topic file not found" });

                string data = File.ReadAllText("Mock/MiscTopic.md");

                Topic topic = Helpers.MarkdownParser.ParseTopic(data);

                return Results.Created($"/api/add-topic/{topic.Id}", new { message = "Topic added to the database", topic });
            }
            catch (Exception ex)
            {
                return Results.InternalServerError(new { message = ex.Message });
            }
            
        })
        .WithName("SaveTopic")
        .WithOpenApi();

        return app;
    }
}
