using TESNS.Models.Authentication;

namespace TESNS.Repositories
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(int id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
