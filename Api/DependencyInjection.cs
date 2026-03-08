using Api.DI;

namespace Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .InjectIdentity(configuration)
            .InjectSwagger()
            .InjectServices()
            .AddSingleton(TimeProvider.System);
    }

    public static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder)
    {
        builder.Logging
            .ClearProviders()
            .AddConsole()
            .AddDebug();

        return builder;
    }
}