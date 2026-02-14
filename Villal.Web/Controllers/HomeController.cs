using Microsoft.AspNetCore.Mvc;
using Villal.Application.Common.Interfaces;
using Villal.Web.ViewModels;

namespace Villal.Web.Controllers
{
    public class HomeController(IUnitOfWork _unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            HomeVM homeVm = new()
            {
                Villas = await _unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now)
            };

            return View(homeVm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeVM homeVM)
        {
            homeVM.Villas = await _unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity");
            foreach (var villa in homeVM.Villas)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }

            return View(homeVM);
        }

        public async Task<IActionResult> GetVillasByDate(int nights, DateOnly checkInDate)
        {
            var villas = await _unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity");
            foreach (var villa in villas)
            {
                if (villa.Id % 2 == 0)
                {
                    villa.IsAvailable = false;
                }
            }

            HomeVM homeVM = new()
            {
                Villas = villas,
                Nights = nights,
                CheckInDate = checkInDate
            };

            return PartialView("_Villas", homeVM);
        }
        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}
