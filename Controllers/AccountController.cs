﻿using Learn_web.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Learn_web.Models;
using Learn_web.ViewModels;
using Learn_web.DataBase;
using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Identity.UI.V3.Pages.Account.Internal;

namespace Learn_web.Controllers
{
    public class AccountController : Controller
    {
        //PersonsContext Users;

        public AccountController(/*PersonsContext Users*/)
        {
            //this.Users = Users;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                

                //Person user = await Users.Users.FirstOrDefaultAsync(i => i.nameUser == model.User && i.userPassword == model.Password);

                if (model.User == "Liliya" && model.Password == "QWEasd321") //ременно измененено (user != null)
                {
                    await Authenticate(model.User); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}