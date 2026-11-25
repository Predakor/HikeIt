using System.Reflection;
using Application.Commons.Abstractions;

namespace Api.DI;

internal static partial class DIextentions
{
    // Run after every registration: the last decorator wraps outermost, so validators run before caches.
    public static IServiceCollection DecorateMarkedServices(this IServiceCollection services)
    {
        var assemblies = sourceArray
            .Select(Assembly.Load)
            .ToArray();

        return services
            .DecorateMarked(assemblies, typeof(ICacheable<>))
            .DecorateMarked(assemblies, typeof(IValidatable<>));
    }

    private static IServiceCollection DecorateMarked(
        this IServiceCollection services,
        Assembly[] assemblies,
        Type marker
    )
    {
        var decorators = assemblies
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type is { IsClass: true, IsAbstract: false })
            .SelectMany(type =>
                type
                    .GetInterfaces()
                    .Where(@interface => @interface.IsGenericType && @interface.GetGenericTypeDefinition() == marker)
                    .Select(@interface => (Service: @interface.GetGenericArguments()[0], Decorator: type))
            );

        foreach (var (service, decorator) in decorators)
        {
            services.Decorate(service, decorator);
        }

        return services;
    }
}
