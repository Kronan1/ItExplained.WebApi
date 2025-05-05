using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Domain.Services.Interfaces;
using it_explained.WebApi.Repository.DbContext;
using MongoDB.Driver;

namespace it_explained.WebApi.Domain.Services
{
    public class TopicService(MongoDbContextService dbContext) : ITopicService
    {
        public async Task SaveTopics(List<Topic> topic)
        {
            var topicsCollection = dbContext.GetCollection<Topic>("topics");

            var exists = await topicsCollection.Find(_ => true).AnyAsync();
            if (!exists)
            {
                Console.WriteLine("The collection is empty.");
            }
            else
            {
                Console.WriteLine("The collection has documents.");
            }

            Console.WriteLine("Adding to db");
            await topicsCollection.InsertManyAsync(topic);
            Console.WriteLine("Added to db");

        }
    }
}
