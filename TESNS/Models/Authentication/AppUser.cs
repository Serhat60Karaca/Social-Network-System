using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using TESNS.Data.Enum;

namespace TESNS.Models.Authentication
{
    public class AppUser : IdentityUser<int>
    {
        public string? PasswordResetToken { get; set; }
        public string? Memleket { get; set; }
        public Gender Cinsiyet { get; set; }
        public string? BirthDate { get; set; }
        public string? ProfilePhoto { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string? CoverPhoto { get; set; }
        public string? ConnId { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public ICollection<Like>? Likes { get; set; }
        
    }
}
