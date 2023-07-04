using Microsoft.EntityFrameworkCore;
using TESNS.Models;

namespace TESNS.Repositories.Concrete
{
    public class UserInteractionRepository : IUserInteractionRepository
    {
        private ApplicationDbContext _context;
        public UserInteractionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserInteraction>> GetUserInteractionsByIdAsync(int userId)
        {
            List<UserInteraction> userInteractions = await _context.UserInteractions
                .Where(i => i.UserId == userId)
                .ToListAsync();

            return userInteractions;
        }
        public bool Add(UserInteraction userInteraction)
        {
            _context.UserInteractions.Add(userInteraction);
            _context.SaveChanges();
            return true;
        }

    }
}
