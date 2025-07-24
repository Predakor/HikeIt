using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Infrastructure.DomainEvents;

public sealed class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher {
    static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
    static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

    public async Task DispatchAsync(
        IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = default
    ) {
        foreach (IDomainEvent domainEvent in domainEvents) {
            using IServiceScope scope = serviceProvider.CreateScope();

            Type domainEventType = domainEvent.GetType();

            Type handlerType = HandlerTypeDictionary.GetOrAdd(
                domainEvent.GetType(),
                et => typeof(IDomainEventHandler<>).MakeGenericType(et)
            );

            IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

            foreach (object? handler in handlers) {
                if (handler is null) {
                    continue;
                }

                var handlerWrapper = HandlerWrapper.Create(handler, domainEventType);
                await handlerWrapper.Handle(domainEvent, cancellationToken);
            }
        }
    }

    abstract class HandlerWrapper {
        public abstract Task Handle(IDomainEvent domainEvent, CancellationToken cancellationToken);

        public static HandlerWrapper Create(object handler, Type domainEventType) {
            Type wrapperType = WrapperTypeDictionary.GetOrAdd(
                domainEventType,
                et => typeof(HandlerWrapper<>).MakeGenericType(et)
            );

            return (HandlerWrapper)Activator.CreateInstance(wrapperType, handler)!;
        }
    }

    sealed class HandlerWrapper<T>(object handler) : HandlerWrapper
        where T : IDomainEvent {
        readonly IDomainEventHandler<T> _handler = (IDomainEventHandler<T>)handler;

        public override async Task Handle(
            IDomainEvent domainEvent,
            CancellationToken cancellationToken
        ) {
            await _handler.Handle((T)domainEvent, cancellationToken);
        }
    }
}
