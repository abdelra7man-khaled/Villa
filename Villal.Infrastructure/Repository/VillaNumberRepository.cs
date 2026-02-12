using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;
using Villal.Infrastructure.Data;

namespace Villal.Infrastructure.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly AppDbContext _context;
        public VillaNumberRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(VillaNumber entity)
        {
            _context.VillaNumbers.Update(entity);
        }

    }
}
