using Microsoft.AspNetCore.Mvc;

namespace Villal.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
