using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebVilla.Data;

namespace WebVilla.Repozitories.Repozitory;

public class RepozitoryService<T>:IRepozitoryService<T> where T:class
{
    private readonly ApplicationContext _context; 
    internal DbSet<T> _dbSet;
    public RepozitoryService(ApplicationContext context)
    {
        _context=context;
        this._dbSet=_context.Set<T>();        
    }

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveAsync();
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, int pageSize = 0, int pageNumber = 1)
    {
        IQueryable<T> query=_dbSet; 
        
        if (filter is not null)
        {
            query=query.Where(filter);
        }
        if (pageSize > 0)
        {
            if (pageSize > 100)
            {
                pageSize = 100;
            }
            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true)
    {
        IQueryable<T> query=_dbSet;

        if (tracked is false)
        {
            query = query.AsNoTracking();
        }
        if (filter is not null)
        {
            query=query.Where(filter);
        }
        
        return await  query.FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(T entity)
    {
       _dbSet.Remove(entity);
       await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}
