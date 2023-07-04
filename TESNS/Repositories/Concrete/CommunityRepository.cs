using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Xml.Linq;
using TESNS.Models;
using TESNS.Models.Authentication;

namespace TESNS.Repositories.Concrete
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly ApplicationDbContext _context;
        public CommunityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Community community)
        {
            _context.Add(community);
            return Save();
        }

        public bool Delete(Community community)
        {
            _context.Remove(community);
            return Save();
        }

        public async Task<IEnumerable<Community>> GetAllCommunities()
        {
            return await _context.Communities.ToListAsync();
        }

        public async Task<Community> GetCommunityWithInviteLink(string inviteLink)
        {
            var community = await _context.Communities.FirstOrDefaultAsync(i => i.InviteLink == inviteLink);
            return community;
        }

        public async Task<IEnumerable<Community>> GetCommunitiesWithName(string name)
        {
            var communities = await _context.Communities.Where(i => i.Name == name).ToListAsync();
            return communities;
        }

        public bool Save()
        {
            var result = _context.SaveChanges();
            return result > 0 ? true : false;
        }

        public bool Update(Community community)
        {
            _context.Update(community);
            return Save();
        }

        public async Task<Community> GetCommunityWithId(int id)
        {
            return await _context.Communities.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Community>> GetCommunitiesWithOwnerId(int id)
        {
            return await _context.Communities.Where(i => i.OwnerId == id).ToListAsync();
        }
    }
}

