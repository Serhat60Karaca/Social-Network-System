using TESNS.Models.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace TESNS.Models
{
    public class Post
    {
        //Attributes
        public int Id { get; set; }
        public string? Header { get; set; }
        public string? Text { get; set; }
        public string? ImagePath { get; set; }
        public string? Video { get; set; }
        public DateTime PublishDate { get; set; } 
        public DateTime EditedDate { get; set; } 
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
        
        //Connections
        [ForeignKey("User")]
        public int UserId { get; set; }
        public AppUser? User { get; set; }

        [ForeignKey("Community")]
        public int CommunityId { get; set; }
        public Community? Community { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
