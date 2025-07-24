using Domain.Interfaces;

namespace Application.Interfaces;
public interface IDomainEventHandler<in T> where T : IDomainEvent {
    Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}
