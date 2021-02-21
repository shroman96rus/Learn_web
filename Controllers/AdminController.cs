using Learn_web.DataBase;
using Learn_web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Learn_web.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        //OrdersContext Users;
        IUsers Users;

        public AdminController(IUsers Users)
        {
            this.Users = Users;
        }

        public IActionResult Index()
        {
            var model = Users.get();
            return View(model);
        }
    }
}
