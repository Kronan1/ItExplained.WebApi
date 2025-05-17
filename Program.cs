using it_explained.WebApi.EndPoints;
using it_explained.WebApi.Domain.Services;
using it_explained.WebApi.Domain.Services.Interfaces;
using it_explained.WebApi.Repository.DbContext;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextJsDev", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000", // Next.js dev server
            "https://localhost:5001", // Swagger UI if needed
            "http://localhost:5000",
            "http://localhost:7222"
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));
builder.Services.AddSingleton<MongoDbContextService>();
builder.Services.AddScoped<ITopicService, TopicService>();

// OpenAPI + Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();


var app = builder.Build();

var apiKey = builder.Configuration["Configuration:ApiKey"];

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;

    if (path.StartsWith("/swagger") || path.StartsWith("/healthy"))
    {
        await next(); // skip API key check
        return;
    }

    // Check for the X-App - Secret header
    if (!context.Request.Headers.TryGetValue("X-Api-Key", out var requestApiKey) || requestApiKey != apiKey)  // Compare with the secret
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden: Invalid API Key");
            return;
        }

    await next();
});


app.UseCors("AllowNextJsDev");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My Minimal API V1");
    options.RoutePrefix = "swagger"; // default
});

// app.MapPrompt();
app.MapTopic();

app.UseHttpsRedirection();
app.MapHealthChecks("/healthy");

app.Run();
