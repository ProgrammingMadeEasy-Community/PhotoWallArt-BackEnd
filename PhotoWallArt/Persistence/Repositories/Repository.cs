using Domain.Entities.Users;
using Domain.Models;
using Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Persistence.Repositories;

public class Repository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
{
    protected readonly ApplicationDbContext DbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public async Task<TEntity?> GetById(TEntityId id)
    {
        return await DbContext.Set<TEntity>()
            .SingleOrDefaultAsync(e => e.Id!.Equals(id));
    }
    
    public void Add(TEntity entity)
    {
        DbContext.Set<TEntity>().Add(entity);
    }

    public async ValueTask AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(List<TEntity> entities, CancellationToken cancellationToken)
    {
        await DbContext.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        DbContext.Set<TEntity>().Update(entity);
    }

    public void Delete(TEntity entity)
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        DbContext.Set<TEntity>().RemoveRange(entities);
    }

    public IQueryable<TEntity> Query => DbContext.Set<TEntity>();

    public IQueryable<TEntity> GetAllByAccountId(Guid accountId)
    {
        return Query.AsNoTracking()
                .Where(a => a.AccountId == accountId);
    }

    public IEnumerable<UserRole> UserRole => 
        (from userRole in DbContext.Set<IdentityUserRole<Guid>>()
        join role in DbContext.Set<Role>() on userRole.RoleId equals role.Id
        select new UserRole(userRole.UserId, role));
}