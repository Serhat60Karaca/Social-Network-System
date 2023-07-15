using TESNS.Models.Authentication;

namespace TESNS.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SendAt { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public virtual ICollection<AppUser> User { get; set; } 
       
    }
}
