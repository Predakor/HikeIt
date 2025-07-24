using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection {
    public static IServiceCollection AddAplication(this IServiceCollection services) {
        return services.AddDomainEvents().AddQueries();
    }

    static IServiceCollection AddDomainEvents(this IServiceCollection services) {
        services.Scan(scan =>
            scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IDomainEventHandler<>)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        return services;
    }

    static IServiceCollection AddQueries(this IServiceCollection services) {
        services.Scan(scan =>
            scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IQueryHandler<,>)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        return services;
    }
}
