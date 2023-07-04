using TESNS.Models;

namespace TESNS.Repositories
{
    public interface IUserInteractionRepository
    {
        Task<List<UserInteraction>> GetUserInteractionsByIdAsync(int userId);
        bool Add(UserInteraction userInteraction);
    }
}
