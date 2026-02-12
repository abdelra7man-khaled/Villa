using Microsoft.AspNetCore.Mvc;
using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Villal.Web.Controllers
{
    public class VillaController(IUnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var villas = await _unitOfWork.Villa.GetAllAsync();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Villa newVilla)
        {
            if (ModelState.IsValid)
            {
                if (newVilla.Image is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(newVilla.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Villa");

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        await newVilla.Image.CopyToAsync(fileStream);
                    }

                    newVilla.ImageUrl = @"\images\Villa\" + fileName;
                }
                else
                {
                    newVilla.ImageUrl = "https://placeholder.co/600x400";
                }

                newVilla.CreatedAt = DateTime.UtcNow;
                newVilla.UpdatedAt = DateTime.UtcNow;

                await _unitOfWork.Villa.AddAsync(newVilla);
                await _unitOfWork.SaveChangesAsync();

                TempData["success"] = "Villa created successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to create new villa";

            return View(newVilla);
        }

        public async Task<IActionResult> Update(int? id)
        {
            var villaToUpdate = await _unitOfWork.Villa.GetAsync(v => v.Id == id);
            if (villaToUpdate == null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(villaToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Villa villaToUpdate)
        {
            if (ModelState.IsValid && villaToUpdate.Id > 0)
            {
                if (villaToUpdate.Image is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villaToUpdate.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\Villa");

                    if (!string.IsNullOrEmpty(villaToUpdate.ImageUrl))
                    {
                        var existingImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villaToUpdate.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(existingImagePath))
                        {
                            System.IO.File.Delete(existingImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        await villaToUpdate.Image.CopyToAsync(fileStream);
                    }

                    villaToUpdate.ImageUrl = @"\images\Villa\" + fileName;
                }

                _unitOfWork.Villa.Update(villaToUpdate);
                await _unitOfWork.SaveChangesAsync();

                TempData["success"] = "Villa updated successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to update villa";

            return View(villaToUpdate);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var villaToDelete = await _unitOfWork.Villa.GetAsync(v => v.Id == id);
            if (villaToDelete == null)
            {
                return RedirectToAction(nameof(Error), "Home");
            }

            return View(villaToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Villa villaToDelete)
        {
            var existingVilla = await _unitOfWork.Villa.GetAsync(v => v.Id == villaToDelete.Id);

            if (existingVilla is not null)
            {
                if (!string.IsNullOrEmpty(villaToDelete.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villaToDelete.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Villa.Remove(existingVilla);
                await _unitOfWork.SaveChangesAsync();

                TempData["success"] = "Villa deleted successfully";

                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "Failed to delete villa";

            return View(existingVilla);
        }
    }
}
