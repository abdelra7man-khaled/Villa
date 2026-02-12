using Villal.Application.Common.Interfaces;
using Villal.Infrastructure.Data;

namespace Villal.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IVillaRepository Villa { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Villa = new VillaRepository(_context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
