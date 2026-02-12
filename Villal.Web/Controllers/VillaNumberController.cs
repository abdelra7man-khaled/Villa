using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Create(VillaNumberVM newVillaNumber)
        {
            bool roomNumberExists = _context.VillaNumbers.Any(vn => vn.VillaNo == newVillaNumber.VillaNumber!.VillaNo);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _context.VillaNumbers.Add(newVillaNumber.VillaNumber!);
                _context.SaveChanges();

                TempData["success"] = "Villa Number created successfully";

                return RedirectToAction(nameof(Index));
            }

            if (roomNumberExists)
            {
                TempData["error"] = "Villa Number already exists";
            }
            else
            {
                TempData["error"] = "Failed to create new villa Number";
            }

            newVillaNumber.Villas = _context.Villas.ToList().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(newVillaNumber);
        }

        public IActionResult Update(int? villaNo)
        {
            VillaNumberVM villaNumberVM = new()
            {
                Villas = _context.Villas.ToList().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(vn => vn.VillaNo == villaNo)
            };


            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaToUpdate)
        {

            if (ModelState.IsValid)
            {
                _context.VillaNumbers.Update(villaToUpdate.VillaNumber!);
                _context.SaveChanges();

                TempData["success"] = "Villa Number updated successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to update new villa Number";


            villaToUpdate.Villas = _context.Villas.ToList().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(villaToUpdate);
        }

        public IActionResult Delete(int? villaNo)
        {
            VillaNumberVM villaNumberVM = new()
            {
                Villas = _context.Villas.ToList().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(vn => vn.VillaNo == villaNo)
            };


            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaToDelete)
        {
            var existingVilla = _context.VillaNumbers.FirstOrDefault(v => v.VillaNo == villaToDelete.VillaNumber!.VillaNo);

            if (existingVilla is not null)
            {
                _context.VillaNumbers.Remove(existingVilla);
                _context.SaveChanges();

                TempData["success"] = "Villa number deleted successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to delete villa number";

            return View(existingVilla);
        }
    }
}
