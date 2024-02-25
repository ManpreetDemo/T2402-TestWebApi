using Microsoft.AspNetCore.Mvc;
using TestWebApi230809.Model;

namespace TestWebApi230809.Services
{
    public interface IStoryService
    {
        /// <summary>
        /// Get best stories in order based on score
        /// </summary>
        /// <param name="n"> Number of stories</param>
        /// <returns></returns>
        Task<IEnumerable<Story>> GetBestOrderedStories(int n);
        void ClearCache();

        int CacheCount();
    }
}