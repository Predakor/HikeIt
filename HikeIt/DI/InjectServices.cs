using Application.Mappers.Implementations;
using Application.Services.Files;
using Domain.TripAnalytics.Interfaces;
using Infrastructure.Parsers;
using Infrastructure.Storage;
using Infrastructure.UnitOfWorks;
using System.Reflection;

namespace Api.DI;

internal static partial class DIextentions {
    public static WebApplicationBuilder InjectServices(this WebApplicationBuilder builder) {
        var assemblies = new[] { "Application", "Infrastructure", "Domain", "Api" }
            .Select(Assembly.Load)
            .ToArray();

        builder
            .Services.AddHttpContextAccessor()
            .InjectMappers()
            .InjectRepositories(assemblies)
            .InjectStorages()
            .InjectServices(assemblies)
            .InjectParsers()
            .InjectUnitOfWorks()
            .InjectQueries(assemblies);

        return builder;
    }

    static IServiceCollection InjectParsers(this IServiceCollection services) {
        services.AddScoped<IGpxParser, GpxParser>();
        return services;
    }

    static IServiceCollection InjectStorages(this IServiceCollection services) {
        services.AddScoped<IFileStorage, FileStorage>();
        return services;
    }

    static IServiceCollection InjectMappers(this IServiceCollection services) {
        services.AddScoped<PeakMapper>();
        services.AddScoped<RegionMapper>();
        return services;
    }

    static IServiceCollection InjectRepositories(
        this IServiceCollection services,
        Assembly[] targetAssemblies
    ) {
        services.Scan(scan =>
            scan.FromAssemblies(targetAssemblies)
                .AddClasses(classes =>
                    classes.Where(type =>
                        type.Name.EndsWith("Repository") && type.IsClass && !type.IsAbstract
                    )
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        return services;
    }

    static IServiceCollection InjectServices(
        this IServiceCollection services,
        Assembly[] targetAssemblies
    ) {
        services.Scan(scan =>
            scan.FromAssemblies(targetAssemblies)
                .AddClasses(classes =>
                    classes.Where(t => t.Name.EndsWith("Service") && t.IsClass && !t.IsAbstract)
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        return services;
    }

    static IServiceCollection InjectUnitOfWorks(this IServiceCollection services) {
        services.AddScoped<ITripAnalyticUnitOfWork, TripAnalyticsUnitOfWork>();
        return services;
    }

    static IServiceCollection InjectQueries(
    this IServiceCollection services,
    Assembly[] targetAssemblies
) {
        services.Scan(scan =>
            scan.FromAssemblies(targetAssemblies)
                .AddClasses(classes =>
                    classes.Where(t => t.Name.EndsWith("QueryService") && t.IsClass && !t.IsAbstract)
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        return services;
    }
}
