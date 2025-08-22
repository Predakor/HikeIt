using Application.Commons.Interfaces;
using Domain.Interfaces;

namespace Infrastructure.Commons.Events.Publishers;

internal sealed class EventPublisher : IEventPublisher {
    readonly IBackgroundQueue _queue;

    public EventPublisher(IBackgroundQueue queue) {
        _queue = queue;
    }

    public async Task PublishAsync(IDomainEvent @event, CancellationToken ct = default) {
        await _queue.EnqueueAsync(@event);
    }

    public async Task PublishAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default) {
        foreach (var @event in events) {
            await PublishAsync(@event, ct);
        }
    }
}
