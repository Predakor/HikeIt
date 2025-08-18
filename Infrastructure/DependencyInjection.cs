using Application.Commons.FileStorage;
using Application.Interfaces;
using Application.Services.Files;
using Domain.Interfaces;
using Domain.TripAnalytics.Interfaces;
using Infrastructure.Data;
using Infrastructure.DomainEvents;
using Infrastructure.Storage;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    ) {
        var assemblies = new[] { typeof(DependencyInjection).Assembly };

        return services
            .AddDatabase(configuration, isDevelopment)
            .AddEventDispatcher()
            .AddRepositories(assemblies)
            .AddUnitOfWork()
            .AddStorages()
            .AddQueryServices(assemblies);
    }

    static IServiceCollection AddEventDispatcher(this IServiceCollection services) {
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        return services;
    }

    static IServiceCollection AddUnitOfWork(this IServiceCollection services) {
        services.AddScoped<ITripAnalyticUnitOfWork, TripAnalyticsUnitOfWork>();
        return services;

    }

    static IServiceCollection AddStorages(this IServiceCollection services) {
        services.AddSingleton<IFileStorage, AzureBlobStorage>();
        services.AddScoped<IGpxFileStorage, FileStorage>();

        return services;
    }

    static IServiceCollection AddRepositories(
        this IServiceCollection services,
        Assembly[] assemblies
    ) {
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
        Assembly[] assemblies
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
