using it_explained.WebApi.Domain.Models;

namespace it_explained.WebApi.Domain.Services.Interfaces
{
    public interface ITopicService
    {
        Task SaveTopics(List<Topic> topic);
    }
}