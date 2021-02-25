using System.ComponentModel.DataAnnotations;

namespace CroppShop.Models.AccountModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Password do not match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
        public string Code { get; set; }
    }
}
