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
    public class DeleteProductByIdCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Result>
        {
            private readonly IProductRepository _repo;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger _logger;
            public DeleteProductByIdCommandHandler(IProductRepository productRepo, IUnitOfWork unitOfWork, ILogger logger)
            {
                 _logger = logger;
                _repo = productRepo;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                using var transaction = _unitOfWork.BeginTransaction();
                try
                {
                    var productToDelete = await _repo.GetById(command.Id);
                    if (productToDelete != null)
                    {
                        productToDelete.DeletedOn = DateTime.UtcNow;
                        productToDelete.DeletedBy = command.Id;

                        _repo.Update(productToDelete);
                        await _unitOfWork.SaveAndCommitAsync(transaction, cancellationToken);

                        return new Result
                        {
                            Message = string.Format(ResponseMessages.RECORDDELETED, productToDelete.Id),
                            Status = true,
                            StatusCode = Statuscodes.DELETED
                        };
                    }

                    return new Result
                    {
                        Message = string.Format(ResponseMessages.RECORDNOTFOUND, command.Id),
                        Status = false,
                    };
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.Error(ex.Message, ex.StackTrace);
                    return new Result
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