using Domain.Common.Abstractions;

namespace Domain.Common.AggregateRoot;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> Events { get; }
    void ClearDomainEvents();
}

public class AggregateRoot<TId, TEntity> : IEntity<TId>, IAggregateRoot, ISoftDeletable<TEntity> where TEntity : AggregateRoot<TId, TEntity>
{
    public required TId Id { get; init; }

    private readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    public bool IsDeleted { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
        Console.WriteLine("adding Domain event, total count:" + Events.Count);
    }

    protected void RemoveDomainEvent(IDomainEvent domainEvent) => _events.Remove(domainEvent);

    public void ClearDomainEvents() => _events.Clear();

    public virtual TEntity Delete(DateTimeOffset time)
    {
        IsDeleted = true;
        DeletedAt = time;
        return (TEntity)this;
    }
}
