namespace Domain.Interfaces;

public interface IDomainEventDispatcher : IEventDispatcher<IDomainEvent> { }

public interface IEventDispatcher<TEvent>
    where TEvent : IEvent {
    Task DispatchAsync(
        IEnumerable<TEvent> domainEvents,
        CancellationToken cancellationToken = default
    );
    Task DispatchAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}
