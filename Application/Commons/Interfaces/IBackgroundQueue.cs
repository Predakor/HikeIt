using Domain.Interfaces;

namespace Application.Commons.Interfaces;

public interface IBackgroundQueue {
    ValueTask EnqueueAsync(IEvent @event);
    ValueTask<IEvent> DequeueAsync(CancellationToken ct);
}
