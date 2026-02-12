using Villal.Application.Common.Interfaces;
using Villal.Infrastructure.Data;

namespace Villal.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IVillaRepository Villa { get; private set; }
        public IVillaNumberRepository VillaNumber { get; private set; }
        public IAmenityRepository Amenity { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Villa = new VillaRepository(_context);
            VillaNumber = new VillaNumberRepository(_context);
            Amenity = new AmenityRepository(_context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
