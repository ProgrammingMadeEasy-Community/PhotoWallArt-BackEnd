using MediatR;

namespace Domain.Shared;

public record DomainEvent(Guid Id) : INotification;