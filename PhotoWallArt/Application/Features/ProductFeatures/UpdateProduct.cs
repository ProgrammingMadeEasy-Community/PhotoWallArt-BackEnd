using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ProductFeatures
{
    public class UpdateProductCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Barcode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Rate { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<Guid>>
        {
            private readonly IProductRepository _repo;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger _logger;
            public UpdateProductCommandHandler(IProductRepository repo, IUnitOfWork unitOfWork, ILogger logger)
            {
                _repo = repo;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }
            public async Task<Result<Guid>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {
               using var transaction = _unitOfWork.BeginTransaction();
                try
                {
                    var productToUpdate = await _repo.GetById(command.Id);
                    if (productToUpdate != null)
                    {
                        productToUpdate.Barcode = command.Barcode;
                        productToUpdate.Name = command.Name;
                        productToUpdate.Rate = command.Rate;
                        productToUpdate.Description = command.Description;
                        
                        _repo.Update(productToUpdate);
                        _unitOfWork.SaveAndCommit(transaction);

                        return new Result<Guid>
                        {
                            Message = string.Format(ResponseMessages.RECORDUPDATED, productToUpdate.Id),
                            Status = true,
                            Data = productToUpdate.Id,
                            StatusCode = Statuscodes.GET_UPDATE
                        };
                    }
                    else
                    {
                        return new Result<Guid>
                        {
                            Message = string.Format(ResponseMessages.RECORDNOTFOUND, command.Id),
                            Status = false,
                            Data = Guid.Empty,
                            StatusCode = Statuscodes.NOTFOUND
                        };
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.Error(ex.Message, ex.StackTrace);
                    return new Result<Guid>
                    {
                        Message = string.Format(ResponseMessages.RECORDUPDATEFAILED, command.Id),
                        Status = false,
                        Data = Guid.Empty,
                        StatusCode = Statuscodes.SERVER_ERROR
                    };

                }

                
            }
        }
    }
}