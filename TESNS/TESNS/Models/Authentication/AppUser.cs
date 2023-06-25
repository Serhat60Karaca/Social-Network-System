using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TESNS.Models.Authentication
{
    public class AppUser : IdentityUser<int>
    {
        public string? PasswordResetToken { get; set; }
        public string? Memleket { get; set; }
        public bool? Cinsiyet { get; set; }  
    }
}
