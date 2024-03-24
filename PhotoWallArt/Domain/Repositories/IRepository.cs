
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRepository<TEntity>
    {
        Task<TEntity?> GetById(Guid id);
        void Add(TEntity entity);
        ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Query { get; }
    }
}
