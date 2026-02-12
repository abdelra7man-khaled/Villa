using System.Linq.Expressions;

namespace Villal.Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task AddAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        void Remove(T entity);
    }
}
