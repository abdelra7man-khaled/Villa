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
