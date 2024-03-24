using Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IPermissionRepository
    {
        ValueTask AddAsync(Permission permission, CancellationToken cancellationToken);
        Task AddRangeAsync(List<Permission> permissions, CancellationToken cancellationToken);
        void Update(Permission permission);
        Task<Permission?> GetById(Guid id);
        IQueryable<Permission> QueryByAccountId(Guid accountId);
        void DeleteRange(IEnumerable<Permission> permissions);
    }
}
