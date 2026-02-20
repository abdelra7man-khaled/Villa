using Microsoft.AspNetCore.Mvc;
using Villal.Application.Common.Interfaces;
using Villal.Application.Common.Utility;
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
        public async Task<IActionResult> GetVillasByDate(int nights, DateOnly checkInDate)
        {
            var villas = (await _unitOfWork.Villa.GetAllAsync(includeProperties: "VillaAmenity")).ToList();
            var villaNumbers = (await _unitOfWork.VillaNumber.GetAllAsync()).ToList();
            var bookedVillas = (await _unitOfWork.Booking.GetAllAsync(b => b.Status == SD.StatusApproved ||
                                                                        b.Status == SD.StatusCheckedIn))
                                                                        .ToList();

            foreach (var villa in villas)
            {
                int roomAvailable = SD.VillaRoomsAvailableCount(villa.Id,
                                                                villaNumbers,
                                                                checkInDate,
                                                                nights,
                                                                bookedVillas);

                villa.IsAvailable = roomAvailable > 0;
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
