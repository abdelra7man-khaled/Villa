using Microsoft.AspNetCore.Mvc;

namespace Villal.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
