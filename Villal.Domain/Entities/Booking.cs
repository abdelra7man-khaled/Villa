using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Villal.Domain.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [ForeignKey("Villa")]
        public int VillaId { get; set; }
        public Villa Villa { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
        public int Nights { get; set; }
        public string? Status { get; set; }

        [Required]
        public DateOnly BookingDate { get; set; }
        [Required]
        public DateOnly CheckInDate { get; set; }
        [Required]
        public DateOnly CheckOutDate { get; set; }

        public bool IsPaymentSuccessful { get; set; }
        public DateTime PaymentDate { get; set; }

        public string? StripeSessionId { get; set; }
        public string? StriptePaymentIntentId { get; set; }

        public DateTime ActualCheckInDate { get; set; }
        public DateTime ActualCheckOutDate { get; set; }

        public int VillaNumber { get; set; }

        [NotMapped]
        public List<VillaNumber> VillaNumbers { get; set; }

    }
}
