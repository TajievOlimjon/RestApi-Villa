using System.Linq.Expressions;

namespace WebVilla.Repozitories.Repozitory;

public interface IRepozitoryService<T>
{
    Task<List<T>> GetAllAsync(Expression<Func<T,bool>>? filter=null);
    Task<T> GetAsync(Expression<Func<T,bool>> filter=null,bool tracked=true);
    Task CreateAsync(T entity);
    Task RemoveAsync(T entity);
    Task SaveAsync();
}
