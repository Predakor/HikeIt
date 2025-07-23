using Domain.Interfaces;

namespace Domain.Common.AggregateRoot;

internal class AggregateRoot<TId> : IEntity<TId> {
    public required TId Id { get; init; }

    readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _events.Add(domainEvent);

    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _events.Remove(domainEvent);

    public void ClearDomainEvents() => _events.Clear();
}
