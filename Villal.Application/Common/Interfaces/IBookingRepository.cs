using Villal.Domain.Entities;

namespace Villal.Application.Common.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        void Update(Booking entity);
        void UpdateStatus(int bookingId, string bookingStatus, int villaNumber);
        void UpdatePaymentIntentId(int bookingId, string sessionId, string paymentIntentId);
    }
}
