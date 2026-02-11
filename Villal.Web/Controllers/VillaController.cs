using Microsoft.AspNetCore.Mvc;
using Villal.Domain.Models;
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

        [HttpPost]
        public IActionResult Create(Villa newVilla)
        {
            if (ModelState.IsValid)
            {
                newVilla.CreatedAt = DateTime.UtcNow;
                newVilla.UpdatedAt = DateTime.UtcNow;

                _context.Villas.Add(newVilla);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(newVilla);
        }
    }
}
