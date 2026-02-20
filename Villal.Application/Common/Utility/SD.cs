using Villal.Domain.Entities;

namespace Villal.Application.Common.Utility
{
    public static class SD
    {
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static int VillaRoomsAvailableCount(int villaId, List<VillaNumber> villaNumbers,
            DateOnly checkInDate, int nights, List<Booking> bookings)
        {
            List<int> bookingInDate = new();
            int finalAvailableRoomsForAllNights = int.MaxValue;

            var roomsInVilla = villaNumbers.Where(vn => vn.VillaId == villaId).Count();

            for (int i = 0; i < nights; ++i)
            {
                var villaBooked = bookings.Where(b => b.CheckInDate <= checkInDate.AddDays(i) &&
                b.CheckOutDate > checkInDate.AddDays(i) && b.VillaId == villaId);

                foreach (var booking in villaBooked)
                {
                    if (!bookingInDate.Contains(booking.Id))
                    {
                        bookingInDate.Add(booking.Id);
                    }
                }

                var totalAvailableRooms = roomsInVilla - bookingInDate.Count();

                if (totalAvailableRooms == 0)
                {
                    return 0;
                }
                else
                {
                    if (finalAvailableRoomsForAllNights > totalAvailableRooms)
                    {
                        finalAvailableRoomsForAllNights = totalAvailableRooms;
                    }
                }
            }

            return finalAvailableRoomsForAllNights;
        }
    }
}
