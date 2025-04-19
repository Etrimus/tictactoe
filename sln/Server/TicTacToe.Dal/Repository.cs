using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.Dal;

public abstract class Repository;

public abstract class Repository<T>(TicTacToeContext context): Repository
    where T: DbEntityBase
{
    protected abstract Func<DbSet<T>, IQueryable<T>> DbSetToQuery { get; }

    public IQueryable<T> Query(bool asNoTracking = false)
    {
        var dbSet = DbSetToQuery(context.Set<T>());

        if (asNoTracking)
        {
            dbSet = dbSet.AsNoTracking();
        }

        return dbSet;
    }

    public Task<bool> ExistAsync(Guid id)
    {
        return context
            .Set<T>()
            .AsNoTracking()
            .AnyAsync(x => x.Id == id);
    }

    public Task<T> GetAsync(Guid id, bool asNoTracking = false)
    {
        var dbSet = DbSetToQuery(context.Set<T>());

        if (asNoTracking)
        {
            dbSet = dbSet.AsNoTracking();
        }

        return dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<T> GetAll()
    {
        return DbSetToQuery(context.Set<T>());
    }

    public async Task<T> AddAsync(T entity)
    {
        var result = await context.AddAsync(entity).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);
        return result.Entity;
    }

    public Task<int> UpdateAsync(T entity)
    {
        context.Update(entity);
        return context.SaveChangesAsync();
    }

    public Task RemoveAsync(T entity)
    {
        context.Remove(entity);
        return context.SaveChangesAsync();
    }
}