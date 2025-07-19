using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        string connectionString,
        bool isProduction
    ) {
        services.AddDbContext<TripDbContext>(options => {
            if (isProduction) {
                options.UseNpgsql(connectionString, x => x.UseNetTopologySuite());
            }
            else {
                options.UseSqlServer(connectionString, x => x.UseNetTopologySuite());
            }
        });

        return services;
    }
}
