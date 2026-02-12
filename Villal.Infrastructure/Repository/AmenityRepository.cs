using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;
using Villal.Infrastructure.Data;

namespace Villal.Infrastructure.Repository
{
    public class AmenityRepository : Repository<Amenity>, IAmenityRepository
    {
        private readonly AppDbContext _context;
        public AmenityRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Amenity entity)
        {
            _context.Amenities.Update(entity);
        }

    }
}
