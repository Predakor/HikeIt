using Application.Commons.Abstractions;
using Domain.Common.Abstractions;
using Domain.Common.AggregateRoot;

namespace Domain.Tests.DomainEvents;

public class DomainEvenTests
{
    private static DummyAggregate GetDummyAggregate => DummyAggregate.Create(Guid.NewGuid(), "");
    [Fact]
    public void AddDomainEventShouldAddEvent()
    {
        var aggregate = DummyAggregate.Create(Guid.NewGuid(), "");
        var domainEvent = new DummyDomainEvent(Guid.NewGuid());

        aggregate.TestAddDomainEvent(domainEvent);

        Assert.Contains(domainEvent, aggregate.Events);
    }

    [Fact]
    public void RemoveDomainEventShouldRemoveEvent()
    {
        var aggregate = GetDummyAggregate;
        var domainEvent = new DummyDomainEvent(Guid.NewGuid());

        aggregate.TestAddDomainEvent(domainEvent);
        aggregate.TestRemoveDomainEvent(domainEvent);

        Assert.DoesNotContain(domainEvent, aggregate.Events);
    }

    [Fact]
    public void ClearDomainEventsShouldEmptyEventList()
    {
        var aggregate = GetDummyAggregate;
        var domainEvent1 = new DummyDomainEvent(Guid.NewGuid());
        var domainEvent2 = new DummyDomainEvent(Guid.NewGuid());

        aggregate.TestAddDomainEvent(domainEvent1);
        aggregate.TestAddDomainEvent(domainEvent2);

        aggregate.ClearDomainEvents();

        Assert.Empty(aggregate.Events);
    }

    [Fact]
    public void CreateShouldRaiseDomainEvent()
    {
        var postId = Guid.NewGuid();
        var post = DomainEvents.DummyAggregate.Create(postId, "Hello World");

        var domainEvent = post.Events.OfType<DummyDomainEvent>().SingleOrDefault();

        Assert.NotNull(domainEvent);
        Assert.Equal(postId, domainEvent!.PostId);
    }
}

public record DummyDomainEvent(Guid PostId) : IDomainEvent;

internal class DummyAggregate : AggregateRoot<Guid, DummyAggregate>
{
    public string Title { get; private set; }

    private DummyAggregate() { }

    public static DummyAggregate Create(Guid id, string title)
    {
        var post = new DummyAggregate { Id = id, Title = title };
        post.TestAddDomainEvent(new DummyDomainEvent(id));
        return post;
    }

    // Only for testing, exposes protected AddDomainEvent
    public void TestAddDomainEvent(IDomainEvent domainEvent) => base.AddDomainEvent(domainEvent);
    public void TestRemoveDomainEvent(IDomainEvent domainEvent) => base.RemoveDomainEvent(domainEvent);
}

internal class DummyPostPublishedHandler : IDomainEventHandler<DummyDomainEvent>
{
    public bool WasCalled { get; private set; }

    public Task Handle(DummyDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        WasCalled = true;
        Console.WriteLine($"Handled domain event for PostId: {domainEvent.PostId}");
        return Task.CompletedTask;
    }
}
