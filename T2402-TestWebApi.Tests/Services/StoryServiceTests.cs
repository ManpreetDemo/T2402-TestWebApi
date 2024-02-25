using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TestWebApi230809.Model;

namespace TestWebApi230809.Services.Tests
{
    [TestClass()]
    public class StoryServiceTests
    {
        private readonly Mock<IStoryApi> _mockStoryApi = new Mock<IStoryApi>();
        private readonly StoryService _storyService;
        public StoryServiceTests()
        {
            _storyService = new StoryService(_mockStoryApi.Object);
        }

        [TestMethod()]
        public async Task GetBestOrderedStories_ReturnsTwoOrderedStories()
        {
            _mockStoryApi.Setup(s => s.GetBestStoryIds()).ReturnsAsync(new List<int> { 20, 10 });
            _mockStoryApi.Setup(s => s.GetStory(It.IsAny<HttpClient>(), It.IsAny<int>())).ReturnsAsync((HttpClient httpClient, int storyId) => new Story{ score = 100-storyId });

            var stories = await _storyService.GetBestOrderedStories(10);

            Assert.IsNotNull(stories);
            Assert.AreEqual(2, stories.Count());            
            Assert.IsTrue(stories.First().score > stories.Last().score);
            Assert.AreEqual(stories.First().score, 90);
            Assert.AreEqual(stories.Last().score, 80);
        }

        [TestMethod()]
        public async Task GetBestOrderedStories_VerifyCacheCountTwo()
        {
            _mockStoryApi.Setup(s => s.GetBestStoryIds()).ReturnsAsync(new List<int> { 20, 10 });
            _mockStoryApi.Setup(s => s.GetStory(It.IsAny<HttpClient>(), It.IsAny<int>())).ReturnsAsync((HttpClient httpClient, int storyId) => new Story { score = 100 - storyId });

            _storyService.ClearCache();
            Assert.AreEqual(0, _storyService.CacheCount());

            var stories = await _storyService.GetBestOrderedStories(10);

            Assert.IsNotNull(stories);
            Assert.AreEqual(2, _storyService.CacheCount());

            stories = await _storyService.GetBestOrderedStories(2);
            Assert.AreEqual(2, _storyService.CacheCount());
        }
        [TestMethod()]
        public async Task GetBestOrderedStories_VerifyClearCache()
        {
            _mockStoryApi.Setup(s => s.GetBestStoryIds()).ReturnsAsync(new List<int> { 20, 10 });
            _mockStoryApi.Setup(s => s.GetStory(It.IsAny<HttpClient>(), It.IsAny<int>())).ReturnsAsync((HttpClient httpClient, int storyId) => new Story { score = 100 - storyId });

            _storyService.ClearCache();
            Assert.AreEqual(0, _storyService.CacheCount());
            var stories = await _storyService.GetBestOrderedStories(10);

            Assert.IsNotNull(stories);
            Assert.AreEqual(2, _storyService.CacheCount());
            _storyService.ClearCache();
            Assert.AreEqual(0, _storyService.CacheCount());
        }
    }
}