using MediatR;

namespace Application.Messaging;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
    public Guid AccountId { get; set; }
}