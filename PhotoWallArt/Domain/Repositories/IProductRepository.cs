using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository
    {
        ValueTask AddAsync(Product product, CancellationToken cancellationToken);
        void Update(Product product);
        Task<Product?> GetById(Guid id);
        IQueryable<Product> GetAllByAccountId(Guid accountId);
    }
}
