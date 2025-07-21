using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        string connectionString
    ) {
        Console.WriteLine("Db connection string in production: " + connectionString);

        services.AddDbContext<TripDbContext>(options => {
            options.UseNpgsql(connectionString, x => x.UseNetTopologySuite());
        });

        return services;
    }
}
