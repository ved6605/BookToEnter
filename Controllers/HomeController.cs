using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Book2Enter.Models;

namespace Book2Enter.Controllers
{
    public class HomeController : Controller
    {
        // Login page (was Pages/Index)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Home page (was Pages/Home)
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [IgnoreAntiforgeryToken]
        public IActionResult Error()
        {
            var model = new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };
            return View(model);
        }
    }
}
