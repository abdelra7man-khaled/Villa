using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using Villal.Application.Common.Interfaces;
using Villal.Application.Common.Utility;
using Villal.Domain.Entities;

namespace Villal.Web.Controllers
{
    public class BookingController(IUnitOfWork unitOfWork) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> FinalizeBooking(Booking booking)
        {
            var villa = await _unitOfWork.Villa.GetAsync(v => v.Id == booking.VillaId);
            booking.TotalPrice = villa.Price * booking.Nights;

            booking.Status = SD.StatusPending;
            booking.BookingDate = DateOnly.FromDateTime(DateTime.Now);

            await _unitOfWork.Booking.AddAsync(booking);
            await _unitOfWork.SaveChangesAsync();

            var domain = Request.Scheme + "://" + Request.Host.Value + "/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Booking/BookingConfirmation?bookingId={booking.Id}",
                CancelUrl = domain + $"Booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };

            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(booking.TotalPrice * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = villa.Name,
                        //Images = new List<string> { domain + villa.ImageUrl }
                    },
                },
                Quantity = 1,
            });

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            _unitOfWork.Booking.UpdatePaymentIntentId(booking.Id, session.Id, session.PaymentIntentId);
            await _unitOfWork.SaveChangesAsync();

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        [Authorize]
        public async Task<IActionResult> BookingConfirmation(int bookingId)
        {
            var booking = await _unitOfWork.Booking.GetAsync(b => b.Id == bookingId, includeProperties: "User,Villa");
            if (booking.Status == SD.StatusPending)
            {
                var service = new SessionService();
                var session = await service.GetAsync(booking.StripeSessionId);
                if (session.PaymentStatus == "paid")
                {
                    _unitOfWork.Booking.UpdateStatus(bookingId, SD.StatusApproved);
                    _unitOfWork.Booking.UpdatePaymentIntentId(bookingId, session.Id, session.PaymentIntentId);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            return View(bookingId);
        }

        [Authorize]
        public async Task<IActionResult> BookingDetails(int bookingId)
        {
            var booking = await _unitOfWork.Booking.GetAsync(b => b.Id == bookingId,
                                    includeProperties: "User,Villa");

            if (booking.VillaNumber == 0 && booking.Status == SD.StatusApproved)
            {
                var availableVillaNumber = await AssignAvailableVillaNumberByVilla(booking.VillaId);

                booking.VillaNumbers = (await _unitOfWork.VillaNumber.GetAllAsync(vn => vn.VillaId == booking.VillaId &&
                                                                        availableVillaNumber.Any(x => x == vn.VillaNo))).ToList();
            }
            return View(booking);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> CheckIn(Booking booking)
        {
            _unitOfWork.Booking.UpdateStatus(booking.Id, SD.StatusCheckedIn, booking.VillaNumber);
            await _unitOfWork.SaveChangesAsync();

            TempData["success"] = "Booking updated successfully";

            return RedirectToAction(nameof(BookingDetails), new { bookingId = booking.Id });
        }


        #region API Calls

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll(string status)
        {
            IEnumerable<Booking> bookings;

            if (User.IsInRole(SD.Role_Admin))
            {
                bookings = await _unitOfWork.Booking.GetAllAsync(includeProperties: "User,Villa");
            }
            else
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)!.Value;

                bookings = await _unitOfWork.Booking.GetAllAsync(b => b.UserId == userId, includeProperties: "User,Villa");
            }

            if (!string.IsNullOrEmpty(status))
            {
                bookings = bookings.Where(b => b.Status!.ToLower().Equals(status.ToLower()));
            }

            return Json(new { data = bookings });
        }


        private async Task<List<int>> AssignAvailableVillaNumberByVilla(int villaId)
        {
            List<int> availableVillaNumbers = new();

            var villaNumbers = await _unitOfWork.VillaNumber.GetAllAsync(vn => vn.VillaId == villaId);

            var checkedInVilla = (await _unitOfWork.Booking.GetAllAsync(b => b.VillaId == villaId &&
                                                                        b.Status == SD.StatusCheckedIn))
                                                                        .Select(b => b.VillaNumber);

            foreach (var villaNumber in villaNumbers)
            {
                if (!checkedInVilla.Contains(villaNumber.VillaNo))
                {
                    availableVillaNumbers.Add(villaNumber.VillaNo);
                }
            }

            return availableVillaNumbers;
        }

        #endregion
    }
}