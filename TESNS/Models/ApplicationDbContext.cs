
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TESNS.Models;
using System.Reflection.Metadata;
using TESNS.Models.Authentication;

namespace TESNS.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommunityUser> CommunityUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
    }
}
