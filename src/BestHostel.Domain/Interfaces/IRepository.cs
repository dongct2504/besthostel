using System.Linq.Expressions;

namespace BestHostel.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracked = true);

    Task CreateAsync(T entity);
    Task RemoveAsync(T entity);

    Task<bool> SaveAsync();
}
