using Application.Commons.Abstractions;
using Scrutor;

namespace Api.Commons.Extentions;

internal static class TypeExtentions
{
    // Decorators implement the service interface too, but are applied by Decorate, not registered.
    public static bool IsDecorator(this Type type)
    {
        return type
            .GetInterfaces()
            .Where(@interface => @interface.IsGenericType)
            .Any(@interface => @interface.GetGenericTypeDefinition() == typeof(IDecorator<>));
    }

    public static IImplementationTypeFilter WithoutDecorators(this IImplementationTypeFilter filter)
    {
        return filter.Where(type => !type.IsDecorator());
    }

    // A service is registered under every interface it implements (a class can serve a Domain interface too), minus the lifetime markers.
    public static IEnumerable<Type> GetServiceInterfaces(this Type type)
    {
        return type
            .GetInterfaces()
            .Where(@interface => @interface != typeof(IService) && @interface != typeof(ISingletonService));
    }
}
