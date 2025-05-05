using it_explained.WebApi.EndPoints;
using AutoMapper;
using it_explained.WebApi.Repository.DbContext;
using it_explained.WebApi.Domain.Services;
using it_explained.WebApi.Domain.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MongoDbContextService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddScoped<TopicService>();

// OpenAPI + Swagger
builder.Services.AddEndpointsApiExplorer(); // Needed for Minimal APIs
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My Minimal API V1");
    options.RoutePrefix = "swagger"; // default
});

app.MapPrompt(
    app.Services.GetRequiredService<IConfiguration>()
    );

app.UseHttpsRedirection();

app.Run();
