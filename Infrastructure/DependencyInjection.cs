using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddDatabase(
            this IServiceCollection services, string connectionString) {
            services.AddDbContext<TripDbContext>(options =>
                options.UseSqlServer(connectionString,
                x => x.UseNetTopologySuite())
                );

            return services;
        }
    }

}