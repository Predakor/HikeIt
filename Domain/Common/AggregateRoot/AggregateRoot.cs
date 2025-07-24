using Domain.Interfaces;

namespace Domain.Common.AggregateRoot;

public interface IAggregateRoot {
    IReadOnlyCollection<IDomainEvent> Events { get; }
    void ClearDomainEvents();
}


public class AggregateRoot<TId> : IEntity<TId>, IAggregateRoot {
    public required TId Id { get; init; }

    readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    protected void TestAddDomainEvent(IDomainEvent domainEvent) => _events.Add(domainEvent);

    protected void TestRemoveDomainEvent(IDomainEvent domainEvent) => _events.Remove(domainEvent);

    public void ClearDomainEvents() => _events.Clear();
}
