using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CroppShop.Controllers
{
    public class BossAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminList()
        {
            return View("BossOperationOverAdmin/AdminList");
        }
    }
}
