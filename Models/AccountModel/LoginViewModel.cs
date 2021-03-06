using System.ComponentModel.DataAnnotations;

namespace CroppShop.Models.AccountModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
