using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Domain.Services;
using it_explained.WebApi.Domain.Services.Interfaces;
using it_explained.WebApi.Helpers;
using it_explained.WebApi.Prompts;
using it_explained.WebApi.Repository.DbContext;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.Text.Json;

namespace it_explained.WebApi.EndPoints
{
    public static class PromptEndPoint
    {
        public static IEndpointRouteBuilder MapPrompt(this IEndpointRouteBuilder app)
        {
            //ITopicService topicService

            app.MapPost("/prompt/generate-topic", async (HttpContext context, IConfiguration configuration, ITopicService topicService) =>
            {
                try
                {
                    var service = topicService;

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "Topics.json");
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("Error: File not found.");
                        return Results.BadRequest($"Error: File {filePath} not found.");
                    }

                    ChatClient client = new(
                        model: "gpt-4.1-mini",
                        apiKey: configuration["Configuration:OpenAi_API_Key"]
                    );

                    string data = File.ReadAllText(filePath);
                    var dictionary = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(data);

                    if (dictionary == null)
                        return Results.NoContent();

                    List<Topic> topicCollection = [];

                    var iterator = 0;
                    foreach (var item in dictionary)
                    {
                        iterator++;

                        string topic = item.Key;
                        string tagsString = string.Join(", ", item.Value);
                        List<string> tags = item.Value;
                        var prompt = Instruction.Prompt + $" Topic = {topic}. Tags = {tags}.";
                        ChatCompletion completion = await client.CompleteChatAsync(prompt);
                        var content = completion.Content[0].Text;

                        topicCollection.Add(TopicHelper.CreateTopic(topic, tags, content));
                        Console.WriteLine($"Generated {iterator} / {dictionary.Count}");

                        if (iterator % 50 == 0 || iterator == dictionary.Count)
                        {
                            if (topicCollection.Count > 0)
                                await service.SaveTopics(topicCollection);

                            topicCollection = [];
                            await Task.Delay(TimeSpan.FromMinutes(2)); // Wait for 2 minutes
                        }
                    }

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
