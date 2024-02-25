using System.Collections.Concurrent;
using TestWebApi230809.Model;

namespace TestWebApi230809.Services
{
    public class StoryService : IStoryService
    {
        private readonly ConcurrentDictionary<int, Story> _storyCache = new ConcurrentDictionary<int, Story>();
        public IStoryApi StoryApi { get; }
        public StoryService(IStoryApi storyApi)
        {
            StoryApi = storyApi;
        }
        public async Task<IEnumerable<Story>> GetBestOrderedStories(int n)
        {
            var stories = new List<Story>();
            var storyIds = await StoryApi.GetBestStoryIds();
            if (storyIds == null) return new List<Story>();
            var tasks = new List<Task<Story>>();
            using (var client = new HttpClient())
            {
                for (int i = 0; i < Math.Min(n, storyIds.Count); i++)
                {
                    var storyId = storyIds[i];
                    var storyTask = GetStoryAsync(client, storyId);
                    tasks.Add(GetStoryAsync(client, storyId));
                }
                stories.AddRange(await Task.WhenAll(tasks));
                stories = stories.OrderByDescending(s => s.score).ToList();
            }
            return stories;
        }
        private async Task<Story> GetStoryAsync(HttpClient httpClient, int storyId)
        {
            if (!_storyCache.TryGetValue(storyId, out var story)) story = await StoryApi.GetStory(httpClient, storyId);
            _ = _storyCache.TryAdd(storyId, story);
            return story;
        }
        public void ClearCache() => _storyCache.Clear();
        public int CacheCount() => _storyCache.Count;
    }
}
