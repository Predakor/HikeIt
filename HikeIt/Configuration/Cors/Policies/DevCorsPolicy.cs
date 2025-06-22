using Api.Configuration.Cors.Interfaces;
using Api.Configuration.Cors.Models;

namespace Api.Configuration.Cors.Policies;

public class DevCorsPolicy(CorsConfig.Development config) : ICorsPolicy {
    public void ApplyCorsPolicy(IServiceCollection services) {
        services.AddCors(options => {
            options.AddPolicy(
                config.Name,
                policy => {
                    policy
                    .WithOrigins("http://localhost:54840")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                }
            );
        });
    }
}
