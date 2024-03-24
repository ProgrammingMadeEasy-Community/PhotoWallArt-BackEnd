using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Common;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductFeatures;

public class CreateProductCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IProductRepository _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public CreateProductCommandHandler(IProductRepository repo, IUnitOfWork unitOfWork, ILogger logger)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var product = new Product()
                {
                    Barcode = command.Barcode,
                    Name = command.Name,
                    Rate = command.Rate,
                    Description = command.Description
                };

                await _repo.AddAsync(product, cancellationToken);
                await _unitOfWork.SaveAndCommitAsync(transaction, cancellationToken);
                return new Result<Guid>()
                {
                    Status = true,
                    Message = string.Format(ResponseMessages.RECORDCREATED, product.Id),
                    Data = product.Id,
                    StatusCode = Statuscodes.CREATED
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.Error(ex.InnerException.ToString(), ex);
                return new Result<Guid>()
                {
                    Status = false,
                    Message = ResponseMessages.RECORDCREATIONFAILED,
                    StatusCode = Statuscodes.SERVER_ERROR
                };
            }
        }
    }
}