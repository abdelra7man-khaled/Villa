using Villal.Application.Common.Interfaces;
using Villal.Application.Common.Utility;
using Villal.Domain.Entities;
using Villal.Infrastructure.Data;

namespace Villal.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly AppDbContext _context;
        public BookingRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Booking entity)
        {
            _context.Bookings.Update(entity);
        }

        public void UpdatePaymentIntentId(int bookingId, string sessionId, string paymentIntentId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking is not null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    booking.StripeSessionId = sessionId;
                }

                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    booking.StriptePaymentIntentId = paymentIntentId;
                    booking.PaymentDate = DateTime.Now;
                    booking.IsPaymentSuccessful = true;
                }
            }
        }

        public void UpdateStatus(int bookingId, string bookingStatus)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking is not null)
            {
                booking.Status = bookingStatus;

                if (bookingStatus == SD.StatusCheckedIn)
                {
                    booking.ActualCheckInDate = DateTime.Now;
                }

                if (bookingStatus == SD.StatusCompleted)
                {
                    booking.ActualCheckOutDate = DateTime.Now;
                }
            }
        }
    }
}
