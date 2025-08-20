using Application.Commons.CacheService;
using Application.Commons.Drafts;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection {
    public static IServiceCollection AddAplication(this IServiceCollection services) {
        return services.AddDomainEvents().AddCacheService().AddQueries().AddDrafts();
    }

    static IServiceCollection AddDomainEvents(this IServiceCollection services) {
        return services.Scan(scan =>
            scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IDomainEventHandler<>)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
    }

    static IServiceCollection AddQueries(this IServiceCollection services) {
        return services.Scan(scan =>
            scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IQueryHandler<,>)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
    }

    static IServiceCollection AddDrafts(this IServiceCollection services) {
        return services.AddSingleton(typeof(IDraftService<>), typeof(MemoryDraftService<>));
    }

    static IServiceCollection AddCacheService(this IServiceCollection services) {
        return services.AddSingleton<ICacheService, InMemoryCacheService>().AddMemoryCache();
    }
}
