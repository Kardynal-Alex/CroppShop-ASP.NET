using System.ComponentModel.DataAnnotations;

namespace CroppShop.Models.AccountModel
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
