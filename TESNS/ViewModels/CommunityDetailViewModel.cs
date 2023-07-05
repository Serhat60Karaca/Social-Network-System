using TESNS.Models;

namespace TESNS.ViewModels
{
    public class CommunityDetailViewModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        
        //public CommunityUser? Users { get; set; }
    }
}
