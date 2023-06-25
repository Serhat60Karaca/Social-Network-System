using Microsoft.AspNetCore.Identity;

namespace TESNS.Models.Authentication
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime CreateDate { get; set; }
    }
}
