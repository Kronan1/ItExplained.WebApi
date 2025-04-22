using AutoMapper;
using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Repository.Initializers;
using MongoDB.Driver;

namespace it_explained.WebApi.EndPoints;

public static class MongoDbSampleEndPoint
{
    public static IEndpointRouteBuilder MapMongoDbSample(this IEndpointRouteBuilder app, DbContextService dbContextService, IMapper mapper)
    {
        app.MapGet("/api/comments", async (HttpContext context) =>
        {
            try
            {
                var collection = dbContextService.MongoClient
                    .GetDatabase("sample_mflix")
                    .GetCollection<Comment>("comments");

                var data = await collection.Find(Builders<Comment>.Filter.Empty).ToListAsync();

                var result = mapper.Map<IEnumerable<Comment>>(data);
                await context.Response.WriteAsJsonAsync(result);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"Internal server error: {ex.Message}");
            }
        })
        .WithName("GetComments")
        .WithOpenApi(); // Ensure the endpoint is included in Swagger

        return app;
    }
}