using System.ComponentModel.DataAnnotations;

namespace TESNS.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
