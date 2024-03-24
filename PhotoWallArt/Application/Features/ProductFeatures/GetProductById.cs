using Azure;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductFeatures
{
    public class GetProductById
    {
        public class GetProductByIdQuery : IRequest<Response>
        {
            public Guid Id { get; set; }
        }
        public class Response : Result<Response>
        {
            public string Name { get; set; } = string.Empty;
            public string Barcode { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public decimal Rate { get; set; }

        }
        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<Response>>
        {
            private readonly IProductRepository _repo;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger _logger;
            public GetProductByIdQueryHandler(IProductRepository repo, IUnitOfWork unitOfWork, ILogger logger)
            {
                _logger = logger;
                _repo = repo;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<Response>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var product = await _repo.GetById(query.Id);
                    if (product != null)
                    {
                        return new Result<Response>()
                        {
                            Data = new Response()
                            {
                                Name = product.Name,
                                Barcode = product.Barcode,
                                Rate = product.Rate,
                                Description = product.Description,
                            },

                            Message = string.Format(ResponseMessages.RECORDFOUND, product.Id),
                            Status = true,
                            StatusCode = Statuscodes.GET_UPDATE
                        };
                    }
                    else
                    {
                        return new Response()
                        {
                            Message = string.Format(ResponseMessages.RECORDNOTFOUND, query.Id),
                            Status = false,
                            StatusCode = Statuscodes.NOTFOUND
                        };
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message, ex.StackTrace);
                    return new Response()
                    {
                        Message = ResponseMessages.SOMEERROROCCURED,
                        Status = false,
                        StatusCode = Statuscodes.SERVER_ERROR
                    };
                }


            }
        }
    }
    

}