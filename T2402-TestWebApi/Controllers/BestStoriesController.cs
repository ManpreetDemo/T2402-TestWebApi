using Microsoft.AspNetCore.Mvc;
using TestWebApi230809.Model;
using TestWebApi230809.Services;

namespace TestWebApi230809.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BestStoriesController : ControllerBase
    {
        private readonly ILogger<BestStoriesController> _logger;
        private readonly IStoryService _storyService;
        public BestStoriesController(ILogger<BestStoriesController> logger, IStoryService storyService)
        {
            this._logger = logger;
            this._storyService = storyService;
        }

        [HttpGet("/GetStories")]
        public async Task<IEnumerable<Story>> Get(int n)
        {
            return await _storyService.GetBestOrderedStories(n);   
        }

        [HttpGet("/ClearStories")]
        public IActionResult ClearStoriesCache()
        {
            _storyService.ClearCache();
            return Ok();
        }
        private void LogInfo(string text) => _logger.LogInformation($"[{DateTime.UtcNow.ToLongTimeString()}]: {text}");
        private void LogError(string message, Exception exception = null)
        {
            if (exception != null)
            {
                _logger.LogError(exception, message);
            }
            else
            {
                _logger.LogError(message);
            }
        }
    }
}
