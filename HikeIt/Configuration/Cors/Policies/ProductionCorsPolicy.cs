using Api.Configuration.Cors.Interfaces;
using Api.Configuration.Cors.Models;

namespace Api.Configuration.Cors.Policies;

public class ProductionCorsPolicy(CorsConfig.Production config) : ICorsPolicy {
    public void ApplyCorsPolicy(IServiceCollection services) {

        Console.WriteLine("Cors origins: " + config.Origins);

        services.AddCors(options => {
            options.AddPolicy(
                config.Name,
                policy => {
                    policy
                        .WithOrigins(config.Origins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("Location");
                }
            );
        });
    }
}
