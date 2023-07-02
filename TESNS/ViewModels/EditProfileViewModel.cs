using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TESNS.Data.Enum;

namespace TESNS.ViewModels
{
    public class EditProfileViewModel
    {
        /*[Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        public string FirstName { get; set; }
        [Display(Name = "Surname")]
        [Required(ErrorMessage = "Surname is required")]
        public string LastName { get; set; }*/
        
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date is required")]
        public string BirthDate { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }
        public string? ProfilePhotoUrl { get; set; }
        public IFormFile? ProfilePhoto { get; set; }
        public string? CoverPhotoUrl { get; set; }
        public IFormFile? CoverPhoto { get; set; }   
    }
}
