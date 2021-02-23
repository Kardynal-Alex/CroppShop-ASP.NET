using Microsoft.AspNetCore.Identity;

namespace CroppShop.Models.AccountModel
{
    public class User : IdentityUser
    {
        public string Role { get; set; }
    }
}
