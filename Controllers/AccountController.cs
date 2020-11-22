using Microsoft.AspNetCore.Mvc;
using BlogMvc.Models;
using BlogMvc.ViewModels;
using Microsoft.EntityFrameworkCore;
using AppContext = BlogMvc.Data.AppContext;
using System.Threading.Tasks;
using UserProfile = System.Security.Claims.ClaimsPrincipal;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppContext db;
        private readonly UserProfile user;
        public AccountController(AppContext _db)
        {
            db = _db;
        }

        /*      [Route("user/{id}")]
              public new IActionResult User(int id)
              {
                  User User = db.Users.FirstOrDefault(u => u.Id == id);
                  return View(User);
              }
        */
        /// <summary>
        /// Регистрация(get запрос)
        /// </summary>
        /// <returns></returns>
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Регистрация(post запрос)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    db.Users.Add(new User { Name = model.Name, Email = model.Email, Password = model.Password });
                    Role userRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "user");
                    if (userRole != null)
                        user.Role = userRole;

                    db.Users.Add(user);
                    await db.SaveChangesAsync();

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        /// <summary>
        /// Вход(get запрос)
        /// </summary>
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Вход(post запрос)
        /// <param name="model"></param>
        /// </summary>
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Name == model.Name || u.Email == model.Name && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("home", "adminPanel");
        }

        /// <summary>
        /// Аутентификация
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
