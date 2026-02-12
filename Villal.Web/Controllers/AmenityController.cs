using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;
using Villal.Web.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Villal.Web.Controllers
{
    public class AmenityController(IUnitOfWork _unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var amenitiess = await _unitOfWork.Amenity.GetAllAsync(includeProperties: "Villa");
            return View(amenitiess);
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            AmenityVM amenityVM = new()
            {
                Villas = villas.ToList().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };

            return View(amenityVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AmenityVM newAmenity)
        {

            if (ModelState.IsValid)
            {
                await _unitOfWork.Amenity.AddAsync(newAmenity.Amenity!);
                await _unitOfWork.SaveChangesAsync();

                TempData["success"] = "Amenity created successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to create new amenity";


            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            newAmenity.Villas = villas.ToList().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(newAmenity);
        }

        public async Task<IActionResult> Update(int? amenityId)
        {
            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            AmenityVM amenityVM = new()
            {
                Villas = villas.ToList().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                Amenity = await _unitOfWork.Amenity.GetAsync(a => a.Id == amenityId)
            };


            if (amenityVM.Amenity is null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(amenityVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(AmenityVM amenityToUpdate)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityToUpdate.Amenity!);
                await _unitOfWork.SaveChangesAsync();

                TempData["success"] = "Amenity updated successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to update amenity";

            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            amenityToUpdate.Villas = villas.ToList().Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });

            return View(amenityToUpdate);
        }

        public async Task<IActionResult> Delete(int? amenityId)
        {
            IEnumerable<Villa> villas = await _unitOfWork.Villa.GetAllAsync();

            AmenityVM amenitiesVM = new()
            {
                Villas = villas.ToList().Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                Amenity = await _unitOfWork.Amenity.GetAsync(a => a.Id == amenityId)
            };


            if (amenitiesVM.Amenity is null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(amenitiesVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(AmenityVM amenityToDelete)
        {
            var existingAmenity = await _unitOfWork.Amenity.GetAsync(a => a.Id == amenityToDelete.Amenity!.Id);

            if (existingAmenity is not null)
            {
                _unitOfWork.Amenity.Remove(existingAmenity);
                await _unitOfWork.SaveChangesAsync();

                TempData["success"] = "Amenity deleted successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to delete amenity";

            return View(existingAmenity);
        }
    }
}
