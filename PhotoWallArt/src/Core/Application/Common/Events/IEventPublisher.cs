using PhotoWallArt.Shared.Events;

namespace PhotoWallArt.Application.Common.Events;
public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}