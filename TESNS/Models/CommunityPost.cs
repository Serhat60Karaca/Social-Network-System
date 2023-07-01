using System.ComponentModel.DataAnnotations.Schema;
using TESNS.Models.Authentication;

namespace TESNS.Models
{
    public class CommunityPost
    {
        public int CommunityPostId { get; set; }

        [ForeignKey("Community")]
        public int CommunityId { get; set; }
        public Community Community { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
