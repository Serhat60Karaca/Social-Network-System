using System.ComponentModel.DataAnnotations;

namespace TESNS.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = " Code is Required")]
        public string Code { get; set; }
        [Required(ErrorMessage = "new password is Reqired")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "password doesnt match")]
        public string ConfirmPassword { get; set; }
    }
}
