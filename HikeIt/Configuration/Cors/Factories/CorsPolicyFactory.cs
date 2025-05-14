using HikeIt.Api.Configuration.Cors.Interfaces;
using HikeIt.Api.Configuration.Cors.Models;
using HikeIt.Api.Configuration.Cors.Policies;

namespace HikeIt.Api.Configuration.Cors.Factories;

public static class CorsPolicyFactory {
    public static ICorsPolicy Create(ICorsConfig config) {
        return config switch {
            CorsConfig.Development devConfig => new DevCorsPolicy(devConfig),
            CorsConfig.Production productionConfig => new ProductionCorsPolicy(productionConfig),
            _ => throw new ArgumentException("Unsupported CORS configuration", nameof(config)),
        };
    }
}
