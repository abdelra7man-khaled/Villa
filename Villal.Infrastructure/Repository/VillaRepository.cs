using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Villal.Application.Common.Interfaces;
using Villal.Domain.Entities;
using Villal.Infrastructure.Data;

namespace Villal.Infrastructure.Repository
{
    public class VillaRepository(AppDbContext _context) : IVillaRepository
    {
        public async Task<IEnumerable<Villa>> GetAllAsync(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Villa> query = _context.Set<Villa>();

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
        {
            IQueryable<Villa> query = _context.Set<Villa>();

            if (filter is not null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task AddAsync(Villa entity)
        {
            await _context.Villas.AddAsync(entity);
        }


        public void Update(Villa entity)
        {
            _context.Villas.Update(entity);
        }

        public void Remove(Villa entity)
        {
            _context.Villas.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
