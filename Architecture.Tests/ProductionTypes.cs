using System.Reflection;
using System.Runtime.CompilerServices;
using Api.Commons.Extentions;

namespace Architecture.Tests;

internal static class ProductionTypes
{
    public static readonly Assembly Core = typeof(IService).Assembly;
    public static readonly Assembly Domain = typeof(Domain.Common.Abstractions.IDomainEventDispatcher).Assembly;
    public static readonly Assembly Application = typeof(Application.DependencyInjection).Assembly;
    public static readonly Assembly Infrastructure = typeof(Infrastructure.DependencyInjection).Assembly;
    public static readonly Assembly Api = typeof(Api.DependencyInjection).Assembly;

    public static readonly Assembly[] All = [Core, Domain, Application, Infrastructure, Api];

    public static IEnumerable<Type> Concrete { get; } =
        All
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type =>
                type is { IsClass: true, IsAbstract: false } && !type.IsDefined(typeof(CompilerGeneratedAttribute))
            )
            .ToArray();

    // Interfaces that opt into registration through a lifetime marker.
    public static IEnumerable<Type> ServiceInterfaces { get; } =
        All
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsInterface && type != typeof(IService) && type != typeof(ISingletonService))
            .Where(type => type.HasServiceMarker())
            .ToArray();

    // Grouping interfaces (IQueryService) are shared bases, so only interfaces no other service interface extends are real contracts.
    public static IEnumerable<Type> LeafServiceInterfaces { get; } =
        ServiceInterfaces
            .Where(@interface =>
                !ServiceInterfaces.Any(other => other != @interface && @interface.IsAssignableFrom(other))
            )
            .ToArray();

    public static IEnumerable<Type> ServiceImplementations { get; } =
        Concrete
            .Where(type => type.HasServiceMarker())
            .Where(type => !type.IsDecorator())
            .ToArray();

    public static IEnumerable<Type> Decorators { get; } =
        Concrete
            .Where(type => type.IsDecorator())
            .ToArray();

    public static string Describe(this IEnumerable<Type> types)
    {
        return string.Join(
            Environment.NewLine,
            types
                .Select(type => type.FullName)
                .Order()
        );
    }
}
