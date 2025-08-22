using Infrastructure.Commons.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Commons.DI;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        string connectionString,
        bool isDevelopement
    ) {
        Console.WriteLine("Db connection string in production: " + connectionString);

        services.AddDbContext<TripDbContext>(options => {
            options.UseNpgsql(connectionString, x => x.UseNetTopologySuite());

            if (isDevelopement) {
                options.EnableSensitiveDataLogging().EnableDetailedErrors();
            }
        });

        return services;
    }
}
