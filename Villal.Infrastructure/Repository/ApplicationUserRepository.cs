using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;
using Villal.Infrastructure.Data;

namespace Villal.Infrastructure.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly AppDbContext _context;
        public ApplicationUserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
