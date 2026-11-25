using Application.Commons.Abstractions;
using Scrutor;

namespace Api.Commons.Extentions;

internal static class TypeExtentions
{
    // Decorators implement the service interface too, but are applied by Decorate, not registered.
    public static bool IsDecorator(this Type type)
    {
        return type
            .GetDecoratorInterfaces()
            .Any();
    }

    // The service a decorator wraps.
    internal static Type GetDecoratedService(this Type decorator)
    {
        return decorator
            .GetDecoratorInterfaces()
            .Single()
            .GetGenericArguments()[0];
    }

    internal static bool HasServiceMarker(this Type type)
    {
        return typeof(IService).IsAssignableFrom(type) || typeof(ISingletonService).IsAssignableFrom(type);
    }

    internal static IImplementationTypeFilter WithoutDecorators(this IImplementationTypeFilter filter)
    {
        return filter.Where(type => !type.IsDecorator());
    }

    // A service is registered under every interface it implements (a class can serve a Domain interface too), minus the lifetime markers.
    internal static IEnumerable<Type> GetServiceInterfaces(this Type type)
    {
        return type
            .GetInterfaces()
            .Where(@interface => @interface != typeof(IService) && @interface != typeof(ISingletonService));
    }

    private static IEnumerable<Type> GetDecoratorInterfaces(this Type type)
    {
        return type
            .GetInterfaces()
            .Where(@interface => @interface.IsGenericType)
            .Where(@interface => @interface.GetGenericTypeDefinition() == typeof(IDecorator<>));
    }
}
