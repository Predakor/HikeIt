using Domain.Common.Abstractions;

namespace Application.Commons.Abstractions;
public interface IDomainEventHandler<in T> where T : IDomainEvent {
    Task Handle(T domainEvent, CancellationToken cancellationToken = default);
}
