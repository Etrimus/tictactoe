using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.Dal
{
    public abstract class Repository
    { }

    public abstract class Repository<T> : Repository where T : DbEntityBase
    {
        private readonly TicTacToeContext _context;

        protected abstract Func<DbSet<T>, IQueryable<T>> DbSetToQuery { get; }

        protected Repository(TicTacToeContext context)
        {
            _context = context;
        }

        protected IQueryable<T> Query()
        {
            return _context.Set<T>();
        }

        public Task<bool> ExistAsync(Guid id)
        {
            return _context.Set<T>()
                .AnyAsync(x => x.Id == id);
        }

        public Task<T> GetAsync(Guid id, bool asNoTracking = false)
        {
            var dbSet = DbSetToQuery(_context.Set<T>());

            if (asNoTracking)
            {
                dbSet = dbSet.AsNoTracking();
            }

            return dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<T> GetAllAsync()
        {
            return DbSetToQuery(_context.Set<T>());
        }

        public async Task<T> AddAsync(T entity)
        {
            var result = await _context.AddAsync(entity).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return result.Entity;
        }

        public Task<int> UpdateAsync(T entity)
        {
            _context.Update(entity);
            return _context.SaveChangesAsync();
        }

        public Task RemoveAsync(T entity)
        {
            _context.Remove(entity);
            return _context.SaveChangesAsync();
        }
    }
}