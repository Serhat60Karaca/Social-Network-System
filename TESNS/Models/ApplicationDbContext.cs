
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TESNS.Models;
using System.Reflection.Metadata;
using TESNS.Models.Authentication;
using Npgsql;

namespace TESNS.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        private static string GetConnectionString()
        {
            var csb = new NpgsqlConnectionStringBuilder
            {
                Host = "localhost",
                Database = "SNSREAL",
                Username = "postgres",
                Password = "82542826",
                Port = 5432,
                KeepAlive = 3000
            };
            return csb.ConnectionString;
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommunityUser> CommunityUsers { get; set; }
        //public DbSet<CommunityPost> CommunityPosts { get; set; }
        public DbSet<UserInteraction> UserInteractions { get; set; } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(GetConnectionString());
            }
        }
    }
}
