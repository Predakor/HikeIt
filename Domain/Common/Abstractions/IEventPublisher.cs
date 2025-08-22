namespace Domain.Common.Abstractions;
public interface IEventPublisher {
    Task PublishAsync(IDomainEvent @event, CancellationToken ct = default);
    Task PublishAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default);
}
