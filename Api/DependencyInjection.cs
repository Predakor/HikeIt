using Api.DI;

namespace Api;

internal static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        return services
            .InjectIdentity()
            .InjectSwagger()
            .InjectServices()
            .AddSingleton(TimeProvider.System);
    }
}
