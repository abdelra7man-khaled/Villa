using Microsoft.AspNetCore.Mvc;
using Villal.Domain.Entities;
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

                TempData["success"] = "Villa created successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to create new villa";

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

        [HttpPost]
        public IActionResult Update(Villa villaToUpdate)
        {
            if (ModelState.IsValid && villaToUpdate.Id > 0)
            {
                _context.Villas.Update(villaToUpdate);
                _context.SaveChanges();

                TempData["success"] = "Villa updated successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to update villa";

            return View(villaToUpdate);
        }

        public IActionResult Delete(int? id)
        {
            var villaToDelete = _context.Villas.FirstOrDefault(v => v.Id == id);
            if (villaToDelete == null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(villaToDelete);
        }

        [HttpPost]
        public IActionResult Delete(Villa villaToDelete)
        {
            var existingVilla = _context.Villas.FirstOrDefault(v => v.Id == villaToDelete.Id);

            if (existingVilla is not null)
            {
                _context.Villas.Remove(existingVilla);
                _context.SaveChanges();

                TempData["success"] = "Villa deleted successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to delete villa";

            return View(existingVilla);
        }
    }
}
