using Infrastructure.Common;
using MediatR;

namespace Application.Messaging;

public interface ICommand : IRequest<Result>
{
    public Guid AccountId { get; set; }
}

public interface ICommand<out TResponse> : IRequest<TResponse>
{
    public Guid AccountId { get; set; }
}