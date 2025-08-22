using Domain.Common.Abstractions;

namespace Application.Commons.Abstractions;

public interface IBackgroundQueue {
    ValueTask EnqueueAsync(IEvent @event);
    ValueTask<IEvent> DequeueAsync(CancellationToken ct);
}
