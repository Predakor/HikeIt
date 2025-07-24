using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool isDevelopment
    ) {
        return services.AddServices().AddDatabase(configuration, isDevelopment);
    }

    static IServiceCollection AddServices(this IServiceCollection services) {
        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        return services;
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
