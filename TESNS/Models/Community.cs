using System.ComponentModel.DataAnnotations.Schema;
using TESNS.Models.Authentication;

namespace TESNS.Models
{
    public class Community
    {
        //Attributes
        public int Id { get; set; }
        public string? Logo  { get; set; }
        public string? Name  { get; set; }
        public string? Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string? InviteLink { get; set; }

        //Connections
        public ICollection<CommunityUser>? Users { get; set; }
        public ICollection<Post>? Posts { get; set; }

        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public AppUser? Owner { get; set; }
    }
}
