using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EleksTask;
using Microsoft.EntityFrameworkCore;
using TourServer.Models;

namespace TourServer
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity 
    {
        readonly ApplicationContext _context;
        readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task Create(TEntity item)
        {
           await _dbSet.AddAsync(item);
        }
        public async Task CreateRange(List<TEntity> items)
        {
            await _dbSet.AddRangeAsync(items);
        }

        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Remove(TEntity item)
        {
            _dbSet.Remove(item);
        }

        public async Task<TEntity> Find(Expression<Func<TEntity, bool>> filter = null,params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().AnyAsync(predicate);
        }

        public async Task<int> Count(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().CountAsync(predicate);
        }

        public async Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,int skip=0,int take=int.MaxValue, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            return await query.Skip(skip).Take(take).ToListAsync();
        }
    }
}


