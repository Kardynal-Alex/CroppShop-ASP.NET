using CroppShop.Models.AccountModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CroppShop.Models.AdditionalModel
{
    public class AdminViewModel
    {
        public List<User> Users { get; set; }
        public AdminRegisterViewModel AdminRegisterViewModel { get; set; }
    }
}
