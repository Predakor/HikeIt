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
        Console.WriteLine(connectionString);

        services.AddDbContext<TripDbContext>(options => {
            if (isProduction) {
                options.UseNpgsql(connectionString, x => x.UseNetTopologySuite());
            }
            else {
                options.UseSqlServer(
                    "Server=localhost\\sqlexpress;Database=hikeit;Trusted_Connection=True;TrustServerCertificate=True;",
                    x => x.UseNetTopologySuite()
                );
            }
        });

        return services;
    }
}
