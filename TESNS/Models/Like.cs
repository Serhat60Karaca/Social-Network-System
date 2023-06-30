using System.ComponentModel.DataAnnotations.Schema;

namespace TESNS.Models
{
    public class Like
    {
        //Attributes
        public int Id { get; set; }
        public char ContentType { get; set; }
        
        //Connections
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post? Post { get; set; }

        [ForeignKey("Comment")]
        public int CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
