using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NETCore.Encrypt.Extensions;
using System.Security.Claims;
using WEB_PROGRAMLAMA_ODEVİ.Entities;
using WEB_PROGRAMLAMA_ODEVİ.Models;

namespace WEB_PROGRAMLAMA_ODEVİ.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        //string hashedPassword;

        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
           _configuration = configuration;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
                //string saltedPassword = model.Password + md5Salt;
                //hashedPassword = saltedPassword.MD5();

                User user = _databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == model.Password);

                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(model.Username), "User is locked.");
                        return View(model);
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty));
                    claims.Add(new Claim("Username", user.Username));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is incorrect.");
                }
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if(_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower())) 
            {
                ModelState.AddModelError(nameof(model.Username), "Username is already exists.");
                View(model);
            }

            //if (ModelState.IsValid)
            //{
            //    string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
            //    string saltedPassword = model.Password + md5Salt;
            //    string hashedPassword = saltedPassword.MD5();
            //}

            User user = new()
            {
                Username = model.Username,
                Password = model.Password
            };

            _databaseContext.Users.Add(user);
            int affectedRowCount = _databaseContext.SaveChanges();

            if(affectedRowCount == 0) 
            {
                ModelState.AddModelError("", "User can not be added.");
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

            return View(model);
        }
        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
