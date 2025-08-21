using Application.Commons.CacheService;
using Application.Commons.Interfaces;
using Application.FileReferences;
using Domain.Interfaces;
using Domain.TripAnalytics.Interfaces;
using Infrastructure.Data;
using Infrastructure.DomainEvents;
using Infrastructure.Services.Caches;
using Infrastructure.Storage;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using System.Reflection;

namespace Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    ) {
        ImmutableArray<Assembly> assemblies = [typeof(DependencyInjection).Assembly];

        return services
            .AddDatabase(configuration, isDevelopment)
            .AddCache()
            .AddStorages()
            .AddEventDispatcher()
            .AddRepositories(assemblies)
            .AddQueryServices(assemblies);
    }

    static IServiceCollection AddEventDispatcher(this IServiceCollection services) {
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        return services;
    }


    static IServiceCollection AddCache(this IServiceCollection services) {
        services.AddMemoryCache();
        return services.AddSingleton<ICache, InMemoryCache>();
    }

    static IServiceCollection AddStorages(this IServiceCollection services) {
        services.AddSingleton<IFileStorage, AzureBlobStorage>();
        services.Decorate<IFileStorage, CachedFileStorageDecorator>();
        return services;
    }

    static IServiceCollection AddRepositories(
        this IServiceCollection services,
        ImmutableArray<Assembly> assemblies
    ) {
        services.AddScoped<ITripAnalyticUnitOfWork, TripAnalyticsUnitOfWork>();

        services.Scan(scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IRepository<,>)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        return services;
    }

    static IServiceCollection AddQueryServices(
        this IServiceCollection services,
        ImmutableArray<Assembly> assemblies
    ) {
        return services.Scan(scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IQueryService)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
    }

    static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    ) {
        string connectionString =
            configuration.GetConnectionString("TripDbCS")
            ?? throw new Exception("DbConnectionString is empty or null");

        services.AddDbContext<TripDbContext>(options => {
            options.UseNpgsql(connectionString, x => x.UseNetTopologySuite());

            if (isDevelopment) {
                options.EnableSensitiveDataLogging().EnableDetailedErrors();
            }
        });

        return services;
    }
}
