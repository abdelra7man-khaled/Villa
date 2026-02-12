using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;
using Villal.Infrastructure.Data;

namespace Villal.Infrastructure.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly AppDbContext _context;
        public VillaRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Villa entity)
        {
            _context.Villas.Update(entity);
        }

    }
}
