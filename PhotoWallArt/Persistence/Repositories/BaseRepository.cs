using Domain.Repositories;
using Domain.Shared;

namespace Persistence.Repositories
{
    public class BaseRepository<TEntity> : Repository<TEntity, Guid>, IRepository<TEntity>
        where TEntity : Entity<Guid>
    {
        public BaseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
