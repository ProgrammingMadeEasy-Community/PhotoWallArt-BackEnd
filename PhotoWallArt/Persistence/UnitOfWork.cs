using System.Data;
using System.Threading;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IDbTransaction BeginTransaction()
    {
        return _dbContext.Database
            .BeginTransaction()
            .GetDbTransaction();
    }

    public void SaveAndCommit(IDbTransaction transaction)
    {
        _dbContext.SaveChangesAsync().Wait();
        transaction.Commit();
    }

    public async Task SaveAndCommitAsync(IDbTransaction transaction, CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
        transaction.Commit();
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}