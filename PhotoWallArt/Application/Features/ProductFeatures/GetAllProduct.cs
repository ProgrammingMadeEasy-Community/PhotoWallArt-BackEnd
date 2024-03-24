using Application.Messaging;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Common;
using Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductFeatures
{
    public class GetAllProduct
    {
        public class GetAllProductsQuery : PagedRequest, IQuery<PagedResult<Response>>
        {
            [JsonIgnore]
            public Guid AccountId { get; set; }
        }

        public class Response 
        {
            public string Name { get; set; } = string.Empty;
            public string Barcode { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Rate { get; set; }

            
        }

        public class Handler(IProductRepository repo, ILogger logger) : IQueryHandler<GetAllProductsQuery, PagedResult<Response>>
        {
            private readonly IProductRepository _repo = repo;
            private readonly ILogger _logger = logger;

            public async Task<PagedResult<Response>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
            {

                return await _repo.GetAllByAccountId(query.AccountId)
               .AsNoTracking()
               .Select(x => new Response
               {
                   Name = x.Name,
                   Barcode = x.Barcode,
                   Description = x.Description,
                   Rate = x.Rate,

               }).PaginateAsync(query.Page, query.PageSize);

               
                   
            }
        }
    }
}