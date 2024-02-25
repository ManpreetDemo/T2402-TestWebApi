using Newtonsoft.Json;
using TestWebApi230809.Model;

namespace TestWebApi230809.Services
{
    public class StoryApi : IStoryApi
    {
        private readonly string BaseApiUrl;
        public StoryApi(IConfiguration configuration)
        {
            BaseApiUrl = configuration["BaseApiUrl"] ?? "";
        }
        public async Task<List<int>> GetBestStoryIds()
        {
            var storyIds = new List<int>();
            using (var httpClient = new HttpClient())
            {
                string bestStoriesUrl = $"{BaseApiUrl}/beststories.json";
                string bestStoryIds = await httpClient.GetStringAsync(bestStoriesUrl);
                storyIds = JsonConvert.DeserializeObject<List<int>>(bestStoryIds);                
            }
            return storyIds ?? new List<int>();
        }
        public async Task<Story> GetStory(HttpClient httpClient, int storyId)
        {
            var storyUrl = $"{BaseApiUrl}/item/{storyId}.json";
            var storyJsonRaw = await httpClient.GetStringAsync(storyUrl);
            var storyJson = JsonConvert.DeserializeObject<StoryJson>(storyJsonRaw);
            return storyJson != null ? MapToStory(storyJson) : new Story();
        }

        private static Story MapToStory(StoryJson story)
        {
            return new Story
            {
                title = story.Title,
                uri = story.Url,
                postedBy = story.By,
                time = UnixTimeStampToDateTime(story.Time).ToString("yyyy-MM-dd HH:mm:ss"),
                score = story.Score,
                commentCount = story.Descendants
            };
        }

        // Helper method to convert Unix timestamp to DateTime
        private static DateTime UnixTimeStampToDateTime(int unixTimeStamp) => new DateTime(1970, 1, 1).AddSeconds(unixTimeStamp);
    }
}
