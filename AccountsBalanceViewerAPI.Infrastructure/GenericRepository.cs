using _4Subsea.ValveTrack.DAL.Interfaces;
using AccountsBalanceViewerAPI.Domain;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AccountsBalanceViewerAPI.Infrastructure;

public class GenericRepository<TEntity> : GenericRepository<TEntity, Guid>, IGenericRepository<TEntity>
    where TEntity : class
{
    public GenericRepository(DataContext context) : base(context) { }

}

public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : class
    where TKey : struct
{
    private readonly DataContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null,
        bool asNoTracking = false)
    {
        IQueryable<TEntity> query = _dbSet;

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (include != null)
        {
            query = include(query);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }

    public virtual async Task<TEntity?> GetByIdAsync(TKey id) => await _dbSet.FindAsync(id);

    public virtual async Task<TEntity> InsertAsync(TEntity entity)
    {
        var newEntity = await _dbSet.AddAsync(entity);
        return newEntity.Entity;
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        var entityToDelete = await _dbSet.FindAsync(id);
        if (entityToDelete != null)
        {
            Delete(entityToDelete);
        }
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (_context.Entry(entityToDelete).State == EntityState.Detached)
        {
            _dbSet.Attach(entityToDelete);
        }
        _dbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        _dbSet.Attach(entityToUpdate);
        _context.Entry(entityToUpdate).State = EntityState.Modified;
    }
}

