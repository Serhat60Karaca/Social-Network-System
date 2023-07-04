using TESNS.Models;
using TESNS.Models.Authentication;

namespace TESNS.Repositories
{
    public interface ICommunityRepository
    {
        Task<IEnumerable<Community>> GetAllCommunities();
        Task<IEnumerable<Community>> GetCommunitiesWithName(string name);
        Task<IEnumerable<Community>> GetCommunitiesWithOwnerId(int id);
        Task<Community> GetCommunityWithInviteLink(string inviteLink);
        Task<Community> GetCommunityWithId(int id);
        
        bool Add(Community community);
        bool Delete(Community community);
        bool Update(Community community);
        bool Save();
    }
}
