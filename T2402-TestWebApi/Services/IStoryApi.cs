using TestWebApi230809.Model;

namespace TestWebApi230809.Services
{
    public interface IStoryApi
    {
        Task<List<int>> GetBestStoryIds();
        Task<Story> GetStory(HttpClient httpClient, int storyId);
    }
}