using System.Reflection;
using Api.Commons.Extentions;
using Application.Commons.Mappers.Implementations;
using Application.Trips.GpxFile.Services;
using Infrastructure.Commons.Parsers;

namespace Api.DI;

internal static partial class DIextentions
{
    private static readonly string[] sourceArray = ["Application", "Infrastructure", "Domain", "Api"];

    public static IServiceCollection InjectServices(this IServiceCollection services)
    {
        var assemblies = sourceArray
            .Select(Assembly.Load)
            .ToArray();

        return services
            .AddHttpContextAccessor()
            .InjectServices(assemblies)
            .InjectMappers()
            .InjectParsers();
    }

    private static IServiceCollection InjectParsers(this IServiceCollection services)
    {
        return services.AddScoped<IGpxParser, GpxParser>();
    }

    private static IServiceCollection InjectMappers(this IServiceCollection services)
    {
        services.AddScoped<PeakMapper>();
        services.AddScoped<RegionMapper>();
        return services;
    }

    private static IServiceCollection InjectServices(this IServiceCollection services, Assembly[] targetAssemblies)
    {
        return services.Scan(scan =>
        {
            scan
                .FromAssemblies(targetAssemblies)
                .AddClasses(
                    classes =>
                        classes
                            .AssignableTo<IService>()
                            .WithoutDecorators(),
                    publicOnly: false
                )
                .As(type => type.GetServiceInterfaces())
                .WithScopedLifetime();

            scan
                .FromAssemblies(targetAssemblies)
                .AddClasses(
                    classes =>
                        classes
                            .AssignableTo<ISingletonService>()
                            .WithoutDecorators(),
                    publicOnly: false
                )
                .As(type => type.GetServiceInterfaces())
                .WithSingletonLifetime();
        });
    }
}
