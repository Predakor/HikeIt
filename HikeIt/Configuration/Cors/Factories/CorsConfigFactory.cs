using HikeIt.Api.Configuration.Cors.Models;

namespace HikeIt.Api.Configuration.Cors.Factories;

public static class CorsConfigFactory {
    public static CorsConfig Create(IHostEnvironment env, IConfiguration configuration) {
        if (env.IsDevelopment()) {
            return new CorsConfig.Development("development");
        }
        if (env.IsProduction()) {
            string[] allowedOrigins =
                configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                ?? throw new NullReferenceException("No allowed origins found");

            return new CorsConfig.Production("production", allowedOrigins);
        }
        return null;
    }
}
