using Microsoft.EntityFrameworkCore;
using TESNS.Models;
using TESNS.Models.Authentication;

namespace TESNS.Repositories.concretes
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            throw new NotImplementedException();
            //return await _context.Users.FindAsync(id);
        }

        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
