using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;

namespace Villal.Web.Controllers
{
    public class BookingController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;


        [Authorize]
        public async Task<IActionResult> FinalizeBooking(int villaId, DateOnly checkInDate, int nights)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            ApplicationUser appUser = await _unitOfWork.ApplicationUser.GetAsync(u => u.Id == userId);
            Booking booking = new()
            {
                VillaId = villaId,
                CheckInDate = checkInDate,
                CheckOutDate = checkInDate.AddDays(nights),
                Nights = nights,
                Villa = await _unitOfWork.Villa.GetAsync(v => v.Id == villaId, includeProperties: "VillaAmenity"),
                UserId = userId ?? string.Empty,
                User = appUser,
                Phone = appUser.PhoneNumber,
                Email = appUser.Email ?? string.Empty,
                Name = appUser?.Name ?? string.Empty
            };
            booking.TotalPrice = booking.Villa.Price * nights;

            return View(booking);
        }
    }
}
