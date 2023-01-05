using Microsoft.AspNetCore.Mvc;
using WEB_PROGRAMLAMA_ODEVİ.Models;

namespace WEB_PROGRAMLAMA_ODEVİ.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Login işlemleri yapılacak
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
            if (ModelState.IsValid)
            {
                // Register işlemleri yapılacak
            }
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
    }
}
