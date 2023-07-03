using System.ComponentModel.DataAnnotations;

namespace TESNS.ViewModels
{
    public class CreateCommunityViewModel
    {
        [Required(ErrorMessage = " Name is required")]
        public string Name { get; set; }
        //[Required(ErrorMessage = " Abbreviation is required")]
        //public string Abbreviation { get; set; }
        public string LogoUrl { get; set; }
        [Required(ErrorMessage = " Logo is required")]
        public IFormFile Logo { get; set; }
        [Required(ErrorMessage = " Description is required")]
        public string? Description  { get; set; }
    }
}
