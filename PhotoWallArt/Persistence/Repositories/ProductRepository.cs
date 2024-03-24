using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ProductRepository : Repository<Product,Guid>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) 
        {
                
        }

        public IQueryable<Product> GetAllByAccountId(Guid accountId)
        {
            return Query.AsNoTracking()
                .Where(x => x.AccountId == accountId);
        }
    }
}
