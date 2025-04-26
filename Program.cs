using it_explained.WebApi.Repository.Initializers;
using it_explained.WebApi.EndPoints;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSingleton<DbContextService>();

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

app.MapWeatherForecast();
app.MapMongoDbSample(
    app.Services.GetRequiredService<DbContextService>()
);
app.MapTopic(
    app.Services.GetRequiredService<DbContextService>(),
    app.Services.GetRequiredService<IConfiguration>()
);

app.UseHttpsRedirection();

app.Run();
