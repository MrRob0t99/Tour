using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TourServer
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<int> Create(TEntity item);
        Task CreateRange(List<TEntity> items);
        Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null,int skip=0,int take=int.MaxValue,
            params Expression<Func<TEntity, object>>[] includes);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        void Remove(TEntity item);
        void Update(TEntity item);
        Task<int> Count(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> Find(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);
    }
}
