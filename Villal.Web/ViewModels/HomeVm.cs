using Villal.Domain.Entities;

namespace Villal.Web.ViewModels
{
    public class HomeVm
    {
        public IEnumerable<Villa> Villas { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public int Nights { get; set; }
    }
}
