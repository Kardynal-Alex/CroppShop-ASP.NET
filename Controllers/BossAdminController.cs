using CroppShop.Models.AccountModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroppShop.Models.AdditionalModel;

namespace CroppShop.Controllers
{
    public class BossAdminController : Controller
    {
        private readonly UserManager<User> userManager;
        public BossAdminController(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminList()
        {
            var users = userManager.Users.Where(x => x.Role == "admin").ToList();
            return View("BossOperationOverAdmin/AdminList",
            new AdminViewModel
            {
                Users = users
            });
        }
    }
}
