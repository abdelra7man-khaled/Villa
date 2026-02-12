using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Villal.Domain.Entities;
using Villal.Infrastructure.Data;
using Villal.Web.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Villal.Web.Controllers
{
    public class VillaNumberController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            var villaNumbers = _context.VillaNumbers.Include(vn => vn.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new()
            {
                Villas = _context.Villas.ToList().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumber newVillaNumber)
        {
            if (ModelState.IsValid)
            {
                _context.VillaNumbers.Add(newVillaNumber);
                _context.SaveChanges();

                TempData["success"] = "Villa Number created successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to create new villa Number";

            return View(newVillaNumber);
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
