using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;
using Villal.Web.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Villal.Web.Controllers
{
    public class VillaNumberController(IUnitOfWork _unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var villaNumbers = await _unitOfWork.VillaNumber.GetAllAsync(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            VillaNumberVM villaNumberVM = new()
            {
                Villas = villas.ToList().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VillaNumberVM newVillaNumber)
        {
            bool roomNumberExists = await _unitOfWork.VillaNumber.AnyAsync(vn => vn.VillaNo == newVillaNumber.VillaNumber!.VillaNo);

            if (ModelState.IsValid && !roomNumberExists)
            {
                await _unitOfWork.VillaNumber.AddAsync(newVillaNumber.VillaNumber!);
                await _unitOfWork.SaveChangesAsync();

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

            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            newVillaNumber.Villas = villas.ToList().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(newVillaNumber);
        }

        public async Task<IActionResult> Update(int? villaNo)
        {
            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            VillaNumberVM villaNumberVM = new()
            {
                Villas = villas.ToList().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = await _unitOfWork.VillaNumber.GetAsync(vn => vn.VillaNo == villaNo)
            };


            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(VillaNumberVM villaToUpdate)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Update(villaToUpdate.VillaNumber!);
                await _unitOfWork.SaveChangesAsync();

                TempData["success"] = "Villa Number updated successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to update new villa Number";

            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            villaToUpdate.Villas = villas.ToList().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(villaToUpdate);
        }

        public async Task<IActionResult> Delete(int? villaNo)
        {
            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            VillaNumberVM villaNumberVM = new()
            {
                Villas = villas.ToList().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = await _unitOfWork.VillaNumber.GetAsync(vn => vn.VillaNo == villaNo)
            };


            if (villaNumberVM.VillaNumber is null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VillaNumberVM villaToDelete)
        {
            var existingVilla = await _unitOfWork.VillaNumber.GetAsync(v => v.VillaNo == villaToDelete.VillaNumber!.VillaNo);

            if (existingVilla is not null)
            {
                _unitOfWork.VillaNumber.Remove(existingVilla);
                await _unitOfWork.SaveChangesAsync();

                TempData["success"] = "Villa number deleted successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to delete villa number";

            return View(existingVilla);
        }
    }
}
