using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TESNS.Data.Enum;
//using TESNS.Migrations;
using TESNS.Models;

namespace TESNS.Services.Concrete
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public RecommendationService(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<Post>> GetRecommendedPostsAsync(int userId, int maxPost)
        {
            var cacheKey = $"RecommendedPosts_{userId}";

            if (_cache.TryGetValue(cacheKey, out List<Post> recommendedPosts))
            {
                return recommendedPosts;
            }

            var lastViewedPosts = await _context.UserInteractions
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.ViewDate)
                .Select(i => i.PostId)
                .ToListAsync();

            var similarPosts = await _context.Posts
                .Where(i => lastViewedPosts.Contains(i.Id))
                .ToListAsync();

            var allPosts = await _context.Posts.ToListAsync();

            recommendedPosts = allPosts
                .Where(i => !lastViewedPosts.Contains(i.Id))
                .OrderByDescending(i => CalculateJaccardSimilarity(i.Categories, similarPosts))
                .Take(maxPost)
                .ToList();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            _cache.Set(cacheKey, recommendedPosts, cacheOptions);

            return recommendedPosts;
        }
        /*
                public async Task<List<Post>> GetRecommendedPostsAsync(int userId, int maxPost)
                {
                    //post id's are listed where user id equals to the userId which was given by async method parameter, ordered by newest to latest
                    var lastViewedPosts = await _context.UserInteractions.Where(i => i.UserId == userId).OrderByDescending(i => i.ViewDate).Select(i => i.PostId).ToListAsync();

                    //list posts that are similar to the recently viewed posts
                    var similarPosts = await _context.Posts.Where(i => lastViewedPosts.Contains(i.Id)).ToListAsync();

                    //list posts that havent been viewed by user and display these post by calculated jaccard similarity rate, max post number indicates how many post will be received
                    //burada hata i.Categories kısmında
                    var recommendedPosts = await _context.Posts.Where(i => !lastViewedPosts.Contains(i.Id)).OrderByDescending(i => CalculateJaccardSimilarity(i.Categories, similarPosts)).Take(maxPost).ToListAsync();
                    return recommendedPosts;

                }
        */

        private double CalculateJaccardSimilarity(List<string> category, List<Post> similarPosts)
        {
            if (similarPosts == null || similarPosts.Count == 0)
            {
                return 0;
            }

            var intersect = similarPosts.SelectMany(i => i.Categories).Intersect(category).Count();
            var union = similarPosts.SelectMany(i => i.Categories).Union(category).Count();

            if (union == 0)
            {
                return 0;
            }

            return (double)intersect / union;
        }


    }
}