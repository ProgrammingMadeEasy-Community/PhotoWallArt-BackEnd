using System.Data;

namespace Domain.Repositories;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task SaveAndCommitAsync(IDbTransaction transaction, CancellationToken cancellationToken = default);
    void SaveAndCommit(IDbTransaction transaction);

    IDbTransaction BeginTransaction(); 
}