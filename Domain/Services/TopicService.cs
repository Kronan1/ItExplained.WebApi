using it_explained.WebApi.Domain.Models;
using it_explained.WebApi.Domain.Services.Interfaces;
using it_explained.WebApi.Repository.DbContext;
using MongoDB.Driver;

namespace it_explained.WebApi.Domain.Services
{
    public class TopicService(MongoDbContextService dbContext) : ITopicService
    {
        public async Task SaveTopics(List<Topic> topics)
        {
            var topicsCollection = dbContext.GetCollection<Topic>("topics");
            var filter = Builders<Topic>.Filter.Empty;

            var exists = await topicsCollection.Find(filter).AnyAsync();
            if (!exists)
            {
                Console.WriteLine("The collection is empty.");
            }
            else
            {
                Console.WriteLine("The collection has documents.");
            }

            List<Topic> filteredTopics = [];
            foreach (var topic in topics)
            {
                var existingTopic = await topicsCollection
                    .Find(t => t.Name == topic.Name)
                    .FirstOrDefaultAsync();

                if (existingTopic == null)
                {
                    // Insert the topic only if it doesn't exist
                    filteredTopics.Add(topic);
                }
            }


            Console.WriteLine("Adding to db");
            await topicsCollection.InsertManyAsync(filteredTopics);
            Console.WriteLine("Added to db");

        }
    }
}
