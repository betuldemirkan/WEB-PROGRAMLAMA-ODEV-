using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NETCore.Encrypt.Extensions;
using System.Security.Claims;
using WEB_PROGRAMLAMA_ODEVİ.Entities;
using WEB_PROGRAMLAMA_ODEVİ.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WEB_PROGRAMLAMA_ODEVİ.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
        
        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
           _configuration = configuration;
        }

        [AllowAnonymous] //authorize olmasın
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = _databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == model.Password);

                if (user != null)
                {
                    if (user.Locked)
                    {
                        ModelState.AddModelError(nameof(model.Username), "User kilitlendi.");
                        return View(model);
                    }

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty));
                    claims.Add(new Claim(ClaimTypes.Role, user.Role));
                    claims.Add(new Claim("Username", user.Username));


                    //Authencitation
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username veya password hatalı.");
                }
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if(_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower())) 
            {
                ModelState.AddModelError(nameof(model.Username), "Bu Username zaten kullanılmakta.");
                View(model);
            }


            User user = new()
            {
                Username = model.Username,
                Password = model.Password
            };

            _databaseContext.Users.Add(user);
            int affectedRowCount = _databaseContext.SaveChanges();

            if(affectedRowCount == 0) 
            {
                ModelState.AddModelError("", "User eklenemedi.");
            }
            else
            {
                return RedirectToAction(nameof(Login));
            }

            return View(model);
        }

        //authorize olsun
        public IActionResult Profile()
        {
            ProfileInfoLoader();
            return View();
        }

        private void ProfileInfoLoader()
        {
            Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

            ViewData["FullName"] = user.FullName;
        }

        [HttpPost]
        public IActionResult ProfileChangeFullName([Required][StringLength(50)] string? fullname)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

                user.FullName = fullname;
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Profile));
            }

            ProfileInfoLoader();
            return View("Profile");
        }

        [HttpPost]
        public IActionResult ProfileChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)
        {
            if (ModelState.IsValid)
            {
                Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user = _databaseContext.Users.SingleOrDefault(x => x.Id == userid);

                string hashedPassword = Convert.ToString(password);

                user.Password = hashedPassword;
                _databaseContext.SaveChanges();

                ViewData["result"] = "Password değiştirildi.";
            }

            ProfileInfoLoader();
            return View("Profile");
        }


        //authorize olsun
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
