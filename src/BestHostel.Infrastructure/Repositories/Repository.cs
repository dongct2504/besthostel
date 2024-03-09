using System.Linq.Expressions;
using BestHostel.Domain.Interfaces;
using BestHostel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BestHostel.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly BestHostelDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(BestHostelDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>(); // ex: DbSet<Hostel> ==> _context.Hostels
    }
    
    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool tracked = true)
    {
        IQueryable<T> query = _dbSet;

        if (!tracked)
        {
            query = query.AsNoTracking();
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _context.AddAsync(entity);
        await SaveAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentException(nameof(entity));
        }

        _context.Remove(entity);
        await SaveAsync();
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}
