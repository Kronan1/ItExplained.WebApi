using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Repository.Initializers;
using OpenAI.Chat;

namespace it_explained.WebApi.EndPoints
{
    public static class PromptEndPoint
    {
        public static IEndpointRouteBuilder MapPrompt(this IEndpointRouteBuilder app, IConfiguration configuration)
        {
            ChatClient client = new(
              model: "gpt-4.1-mini",
              apiKey: configuration["Configuration:OpenAi_API_Key"]
            );

            app.MapPost("/prompt/generate-topic", async (HttpContext context, string requestedTopic) =>
            {
                try
                {
                    ChatCompletion completion = await client.CompleteChatAsync("Say 'this is a test.'");
                    Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.InternalServerError(new { message = ex.Message });
                }

            })
            .WithName("Prompt")
            .WithOpenApi();

            return app;
        }
    }
}
