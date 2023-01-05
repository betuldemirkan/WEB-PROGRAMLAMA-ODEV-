using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB_PROGRAMLAMA_ODEVİ.Controllers
{ 
    [Authorize (Roles="Admin")]
    public class AdminController : Controller
    {
       
        public IActionResult Index()
        {
            return View();
        }
    }
}
