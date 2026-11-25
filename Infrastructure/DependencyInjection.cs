using System.Collections.Immutable;
using System.Reflection;
using Application.Commons.Abstractions;
using Application.FileReferences;
using Domain.Common.Abstractions;
using Domain.Trips.Analytics.Root.Interfaces;
using Infrastructure.Commons.Caches;
using Infrastructure.Commons.Databases;
using Infrastructure.Commons.Events.Dispatchers;
using Infrastructure.Commons.Events.Dispatchers.Decorators;
using Infrastructure.Commons.Events.Publishers;
using Infrastructure.Commons.Events.Queues;
using Infrastructure.Commons.Events.Workers;
using Infrastructure.Commons.Storage;
using Infrastructure.Commons.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    )
    {
        ImmutableArray<Assembly> assemblies = [typeof(DependencyInjection).Assembly];

        return services
            .AddDatabase(configuration, isDevelopment)
            .AddMemoryCache()
            .AddSingleton<ICache, InMemoryCache>()
            .AddSingleton<IFileStorage, AzureBlobStorage>()
            .AddEvents()
            .AddRepositories(assemblies);
    }

    private static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.Decorate<IDomainEventDispatcher, LoggedEventDispatcherDecorator>();

        services.AddSingleton<IBackgroundQueue, InMemoryBackgroundQueue>();
        services.AddSingleton<IEventPublisher, EventPublisher>();

        services.AddHostedService<BackgroundEventWorker>();
        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services,
        ImmutableArray<Assembly> assemblies
    )
    {
        return services
            .AddScoped<ITripAnalyticUnitOfWork, TripAnalyticsUnitOfWork>()
            .Scan(scan =>
                scan
                    .FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IRepository<,>)), publicOnly: false)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
            );
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    )
    {
        string connectionString =
            configuration.GetConnectionString("TripDbCS") ?? throw new Exception("DbConnectionString is empty or null");

        services.AddDbContext<TripDbContext>(options =>
        {
            options.UseNpgsql(
                connectionString,
                builder =>
                {
                    builder.UseNetTopologySuite();
                    builder.MigrationsAssembly(typeof(TripDbContext).Assembly.FullName);
                }
            );

            if (isDevelopment)
            {
                options
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });

        return services;
    }
}
