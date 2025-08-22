using Application.Commons.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Commons.Events.Dispatchers;

internal sealed class LoggedEventDispatcherDecorator
    : IDomainEventDispatcher,
        IDecorator<IDomainEventDispatcher> {
    readonly IDomainEventDispatcher _inner;
    readonly ILogger<IEvent> _logger;

    public LoggedEventDispatcherDecorator(IDomainEventDispatcher inner, ILogger<IEvent> logger) {
        _inner = inner;
        _logger = logger;
    }

    public async Task DispatchAsync(
        IDomainEvent domainEvent,
        CancellationToken cancellationToken = default
    ) {
        _logger.LogDebug("{event}", domainEvent.ToString());
        await _inner.DispatchAsync(domainEvent, cancellationToken);
    }

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default
    ) {
        foreach (var @event in domainEvents) {
            await DispatchAsync(@event, cancellationToken);
        }
    }
}
