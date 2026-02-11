using Microsoft.AspNetCore.Mvc;
using Villal.Domain.Models;
using Villal.Infrastructure.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public IActionResult Update(int? id)
        {
            var villaToUpdate = _context.Villas.FirstOrDefault(v => v.Id == id);
            if (villaToUpdate == null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(villaToUpdate);
        }
    }
}
