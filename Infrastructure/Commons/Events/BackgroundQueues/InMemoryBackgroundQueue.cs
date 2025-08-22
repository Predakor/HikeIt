using Application.Commons.Interfaces;
using Domain.Interfaces;
using System.Threading.Channels;

namespace Infrastructure.Commons.Events.BackgroundQueues;

internal class InMemoryBackgroundQueue : IBackgroundQueue {
    readonly Channel<IEvent> _queue = Channel.CreateUnbounded<IEvent>();

    public async ValueTask EnqueueAsync(IEvent @event) {
        await _queue.Writer.WriteAsync(@event);
    }

    public ValueTask<IEvent> DequeueAsync(CancellationToken ct) {
        return _queue.Reader.ReadAsync(ct);
    }
}
