using System.ComponentModel.DataAnnotations.Schema;
using TESNS.Models.Authentication;

namespace TESNS.Models
{
    public class Comment
    {
        //Attributes
        public int Id { get; set; }
        public string? CommentText { get; set; }

        //Connections
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post? Post { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}
