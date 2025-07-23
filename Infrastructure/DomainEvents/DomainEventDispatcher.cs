using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.DomainEvents;

public sealed class DomainEventDispatcher : IDomainEventDispatcher {
    readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default
    ) {
        foreach (IDomainEvent domainEvent in domainEvents) {
            using IServiceScope scope = _serviceProvider.CreateScope();

            Type handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (object? handler in handlers) {
                if (handler is null) {
                    continue;
                }


                MethodInfo? handleMethod = handlerType.GetMethod("Handle");

                if (handleMethod is not null) {
                    await (Task)handleMethod.Invoke(handler, [domainEvents, cancellationToken])!;
                }
            }
        }
    }
}
