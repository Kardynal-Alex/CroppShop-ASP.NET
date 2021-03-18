using Microsoft.AspNetCore.Identity;

namespace CroppShop.Models.AccountModel
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
    }
}
