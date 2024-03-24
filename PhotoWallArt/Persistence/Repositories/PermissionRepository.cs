using Domain.Entities.Users;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class PermissionRepository : Repository<Permission, Guid>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext dbContext) 
            : base(dbContext)
        {
        }

        public IQueryable<Permission> QueryByAccountId(Guid accountId)
        {
            return Query.Where(p => p.AccountId == accountId);
        }
    }
}
