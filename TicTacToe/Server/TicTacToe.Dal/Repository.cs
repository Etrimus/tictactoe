using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicTacToe.Domain;

namespace TicTacToe.Dal
{
    public abstract class Repository<T> where T : DbEntityBase
    {
        private readonly TicTacToeContext _context;

        protected Repository(TicTacToeContext context)
        {
            _context = context;
        }

        public Task<T> GetAsync(Guid id)
        {
            return _context
                .Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<T> GetAllAsync()
        {
            return _context.Set<T>();
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