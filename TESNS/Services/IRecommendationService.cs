using Microsoft.EntityFrameworkCore;
using TESNS.Models;

namespace TESNS.Services
{
    public interface IRecommendationService
    {
        Task<List<Post>> GetRecommendedPostsAsync(int userId, int maxPost);
    }
}
