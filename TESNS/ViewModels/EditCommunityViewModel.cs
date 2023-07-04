namespace TESNS.ViewModels
{
    public class EditCommunityViewModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public IFormFile? Logo { get; set; }
    }
}
