using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using TESNS.Data.Enum;

namespace TESNS.ViewModels
{
    public class AppUserViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Birth Date")]
        [Required(ErrorMessage = "Birth Date is required")]
        public string BirthDate { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }
    }
}
