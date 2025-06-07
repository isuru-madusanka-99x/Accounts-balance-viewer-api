using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace _4Subsea.ValveTrack.DAL.Interfaces;

public interface IGenericRepository<TEntity> : IGenericRepository<TEntity, Guid>
    where TEntity : class
{ }

public interface IGenericRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool asNoTracking = false);
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<TEntity> InsertAsync(TEntity entity);
    Task DeleteAsync(TKey id);
    void Delete(TEntity entityToDelete);
    void Update(TEntity entityToUpdate);
}
