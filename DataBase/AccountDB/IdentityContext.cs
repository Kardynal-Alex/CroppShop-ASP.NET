using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CroppShop.Models.AccountModel;

namespace CroppShop.DataBase.AccountDB
{
    public class IdentityContext : IdentityDbContext<User>
    {
        public IdentityContext(DbContextOptions<IdentityContext> opt) : base(opt) { }
    }
}
