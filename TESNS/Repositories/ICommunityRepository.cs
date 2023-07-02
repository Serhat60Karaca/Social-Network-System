using TESNS.Models;

namespace TESNS.Repositories
{
    public interface ICommunityRepository
    {
        Task<IEnumerable<Community>> GetAllCommunities();
        Task<IEnumerable<Community>> GetCommunitiesWithName(string name);
        Task<Community> GetCommunityWithInviteLink(string inviteLink);
        bool Add(Community community);
        bool Delete(Community community);
        bool Update(Community community);
        bool Save();
    }
}
