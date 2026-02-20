using Microsoft.AspNetCore.Mvc;
using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;

namespace Villal.Web.Controllers
{
    public class BookingController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IActionResult> FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
        {
            Booking booking = new()
            {
                VillaId = villaId,
                CheckInDate = checkInDate,
                CheckOutDate = checkInDate.AddDays(nights),
                Nights = nights,
                Villa = await _unitOfWork.Villa.GetAsync(v => v.Id == villaId, includeProperties: "VillaAmenity")
            };
            booking.TotalPrice = booking.Villa.Price * nights;

            return View(booking);
        }
    }
}
