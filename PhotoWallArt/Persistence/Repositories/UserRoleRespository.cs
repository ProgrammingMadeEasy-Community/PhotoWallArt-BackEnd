using Domain.Entities.Users;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    internal sealed class UserRoleRespository : IUserRoleRespository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRespository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<IdentityUserRole<Guid>> Query => 
            _context.Set<IdentityUserRole<Guid>>();
    }
}
