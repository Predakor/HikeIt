using Domain.Common.Abstractions;

namespace Application.Commons.Abstractions;

public interface IBackgroundQueue : ISingletonService
{
    ValueTask EnqueueAsync(IEvent @event);
    ValueTask<IEvent> DequeueAsync(CancellationToken ct);
}
