using Microsoft.EntityFrameworkCore;
using TESNS.Data.Enum;
using TESNS.Migrations;
using TESNS.Models;

namespace TESNS.Services.Concrete
{
    public class RecommendationService : IRecommendationService
    {
        public ApplicationDbContext _context;
        public RecommendationService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Post>> GetRecommendedPostsAsync(int userId, int maxPost, DbContextOptions<ApplicationDbContext> options)
        {
            using (var context = new ApplicationDbContext(options)) // Create a new instance of ApplicationDbContext
            {
                var lastViewedPosts = await context.UserInteractions
                    .Where(i => i.UserId == userId)
                    .OrderByDescending(i => i.ViewDate)
                    .Select(i => i.PostId)
                    .ToListAsync();

                var similarPosts = await context.Posts
                    .Where(i => lastViewedPosts.Contains(i.Id))
                    .ToListAsync();

                var recommendedPosts = context.Posts
                    .Where(i => !lastViewedPosts.Contains(i.Id))
                    .AsEnumerable() // Switch to client-side evaluation
                    .OrderByDescending(i => CalculateJaccardSimilarity(i.Categories, similarPosts))
                    .Take(maxPost)
                    .ToList();

                return recommendedPosts;
            }
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