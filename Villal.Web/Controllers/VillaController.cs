using Microsoft.AspNetCore.Mvc;
using Villal.Infrastructure.Data;

namespace Villal.Web.Controllers
{
    public class VillaController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            var villas = _context.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
