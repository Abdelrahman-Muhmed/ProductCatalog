using Microsoft.AspNetCore.Mvc;
using ProductCategory_PL.Models;
using System.Diagnostics;

namespace ProductCatalog.Controllers
{
    public class HomeController : Controller
    {
 
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Login", "Auth");       
		}

    }
}
