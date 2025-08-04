using Application.Commons.Drafts;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection {
    public static IServiceCollection AddAplication(this IServiceCollection services) {
        return services.AddDomainEvents().AddQueries().AddMemoryCache().AddDrafts();
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

    static IServiceCollection AddDrafts(this IServiceCollection services) {
        services.AddSingleton(typeof(IDraftService<>), typeof(MemoryDraftService<>));

        return services;
    }
}
