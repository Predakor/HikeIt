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

    protected void AddDomainEvent(IDomainEvent domainEvent) {
        _events.Add(domainEvent);
        Console.WriteLine("adding Domain event, total count:" + Events.Count);
    }

    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _events.Remove(domainEvent);

    public void ClearDomainEvents() => _events.Clear();
}
