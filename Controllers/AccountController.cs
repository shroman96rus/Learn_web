using Learn_web.Repository;
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
        readonly OrdersContext Users;

        public AccountController(OrdersContext Users)
        {
            this.Users = Users;
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
                

                User user = await Users.Users.FirstOrDefaultAsync(i => i.nameUser == model.User && i.userPassword == model.Password);

                if (user != null) //ременно измененено (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Login", "Некорректные логин и(или) пароль");

            }
            return View(model);
        }



        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.nameUser),
                new Claim(ClaimTypes.Role, user.role)
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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await Users.Users.FirstOrDefaultAsync(u => u.nameUser == model.nameUser);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User
                    {
                        nameUser = model.nameUser,
                        userPassword = model.userPassword,
                        role = model.role,

                    };
                    Users.Users.Add(user);
                    await Users.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }



    }
}
