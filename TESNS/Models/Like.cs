using System.ComponentModel.DataAnnotations.Schema;
using TESNS.Models.Authentication;

namespace TESNS.Models
{
    public class Like
    {
        //Attributes
        public int Id { get; set; }
        public char? ContentType { get; set; }
        
        //Connections
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post? Post { get; set; }

        [ForeignKey("Comment")]
        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }

        [ForeignKey("AppUser")]
        public int UserId { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
