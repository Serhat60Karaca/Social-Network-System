using System.ComponentModel.DataAnnotations.Schema;

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

        public ICollection<Like> Likes { get; set; }
    }
}
