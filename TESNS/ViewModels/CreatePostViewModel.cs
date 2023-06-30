namespace TESNS.ViewModels
{
    public class CreatePostViewModel
    {
        public string? Header { get; set; }
        public string? Text { get; set; }
        public IFormFile ImagePath { get; set; }
        public string? Video { get; set; }
    }
}
