using HikeIt.Api.Configuration.Cors.Interfaces;
using HikeIt.Api.Configuration.Cors.Models;

namespace HikeIt.Api.Configuration.Cors.Policies;

public class DevCorsPolicy(CorsConfig.Development config) : ICorsPolicy {
    public void ApplyCorsPolicy(IServiceCollection services) {
        services.AddCors(options => {
            options.AddPolicy(
                config.Name,
                policy => {
                    policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                }
            );
        });
    }
}
