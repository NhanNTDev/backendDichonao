using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiCho.Core.BaseConnect
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<IDbContextTransaction> BeginTransaction(CancellationToken cancellationToken = default);
        int Count();
        TEntity Get<TKey>(TKey id);
        Task<TEntity> GetAsyn<TKey>(TKey id);
        IQueryable<TEntity> Get();
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault();
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsyn();
        Task<TEntity> FirstOrDefaultAsyn(Expression<Func<TEntity, bool>> predicate);
        TEntity LastOrDefault();
        void Create(TEntity entity);
        Task CreateAsyn(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsyn(IEnumerable<TEntity> entities);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
