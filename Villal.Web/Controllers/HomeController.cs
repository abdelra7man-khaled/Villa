using Microsoft.AspNetCore.Mvc;
using Villal.Application.Common.Interfaces;
using Villal.Web.ViewModels;

namespace Villal.Web.Controllers
{
    public class HomeController(IUnitOfWork _unitOfWork) : Controller
    {
        public async Task<IActionResult> Index()
        {
            HomeVm homeVm = new()
            {
                Villas = await _unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now)
            };

            return View(homeVm);
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
