using TESNS.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace TESNS.Models
{
    public class CommunityUser
    {
        public int CommunityUserId { get; set; }
        
        [ForeignKey("Community")]
        public int CommunityId { get; set; }
        public Community Community { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public AppUser User { get; set; }
    }
}
