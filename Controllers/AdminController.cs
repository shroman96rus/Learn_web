using Learn_web.DataBase;
using Learn_web.Interfaces;
using Learn_web.Models;
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
        private readonly IUsers Users;

        public AdminController(IUsers Users)
        {
            this.Users = Users;
        }

        public IActionResult Index()
        {
            var model = Users.get();
            return View(model);
        }

        public IActionResult Update(int id)
        {
            User updateUser = Users.GetUser(id);
            return View(updateUser);
        }

        [HttpPost]
        public IActionResult Update(User updateUser)
        {
            if (ModelState.IsValid)
            {
                Users.UpdateUser(updateUser);
                return RedirectToAction("Index");
            }
            else
            {
                return View(updateUser);
            }
           
        }

       
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                Users.DeleteUser(id);
                
            }
           
            return RedirectToAction("Index");

        }
    }
}
