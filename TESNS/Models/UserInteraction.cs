using System.ComponentModel.DataAnnotations.Schema;
using TESNS.Models.Authentication;

namespace TESNS.Models
{
    public class UserInteraction
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public AppUser? User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public DateTime ViewDate { get; set; }
    }
}
