using Application.Mappers.Implementations;
using Application.Services.Files;
using Domain.TripAnalytics.Interfaces;
using Infrastructure.Parsers;
using Infrastructure.Storage;
using Infrastructure.UnitOfWorks;
using System.Reflection;

namespace Api;

public static class DependencyInjection {
    public static WebApplicationBuilder InjectServices(this WebApplicationBuilder builder) {
        var assemblies = new[] { "Application", "Infrastructure", "Domain", "Api" }
            .Select(Assembly.Load)
            .ToArray();

        builder
            .InjectMappers()
            .InjectRepositories(assemblies)
            .InjectStorages()
            .InjectServices(assemblies)
            .InjectParsers()
            .InjectUnitOfWorks();

        return builder;
    }

    static WebApplicationBuilder InjectParsers(this WebApplicationBuilder builder) {
        builder.Services.AddScoped<IGpxParser, GpxParser>();
        return builder;
    }

    static WebApplicationBuilder InjectStorages(this WebApplicationBuilder builder) {
        builder.Services.AddScoped<IFileStorage, FileStorage>();
        return builder;
    }

    static WebApplicationBuilder InjectMappers(this WebApplicationBuilder builder) {
        builder.Services.AddScoped<PeakMapper>();
        builder.Services.AddScoped<RegionMapper>();
        return builder;
    }

    static WebApplicationBuilder InjectRepositories(
        this WebApplicationBuilder builder,
        Assembly[] targetAssemblies
    ) {
        builder.Services.Scan(scan =>
            scan.FromAssemblies(targetAssemblies)
                .AddClasses(classes =>
                    classes.Where(type =>
                        type.Name.EndsWith("Repository") && type.IsClass && !type.IsAbstract
                    )
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        return builder;
    }

    static WebApplicationBuilder InjectServices(
        this WebApplicationBuilder builder,
        Assembly[] targetAssemblies
    ) {
        builder.Services.Scan(scan =>
            scan.FromAssemblies(targetAssemblies)
                .AddClasses(classes =>
                    classes.Where(t => t.Name.EndsWith("Service") && t.IsClass && !t.IsAbstract)
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
        return builder;
    }

    static WebApplicationBuilder InjectUnitOfWorks(this WebApplicationBuilder builder) {
        builder.Services.AddScoped<ITripAnalyticUnitOfWork, TripAnalyticsUnitOfWork>();
        return builder;
    }
}
