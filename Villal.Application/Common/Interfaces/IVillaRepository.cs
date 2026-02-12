using System.Linq.Expressions;
using Villal.Domain.Entities;

namespace Villal.Application.Common.Interfaces
{
    public interface IVillaRepository
    {
        Task<IEnumerable<Villa>> GetAllAsync(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null);
        Task<Villa> GetAsync(Expression<Func<Villa, bool>> filter, string? includeProperties = null);
        Task AddAsync(Villa entity);
        void Update(Villa entity);
        void Remove(Villa entity);
        Task SaveChangesAsync();
    }
}
