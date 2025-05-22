using Api.Configuration.Cors.Interfaces;
using Api.Configuration.Cors.Models;
using Api.Configuration.Cors.Policies;

namespace Api.Configuration.Cors.Factories;

public static class CorsPolicyFactory {
    public static ICorsPolicy Create(ICorsConfig config) {
        return config switch {
            CorsConfig.Development devConfig => new DevCorsPolicy(devConfig),
            CorsConfig.Production productionConfig => new ProductionCorsPolicy(productionConfig),
            _ => throw new ArgumentException("Unsupported CORS configuration", nameof(config)),
        };
    }
}
