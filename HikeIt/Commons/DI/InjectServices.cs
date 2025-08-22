using Application.Commons.Mappers.Implementations;
using Application.Trips.GpxFile.Services;
using Infrastructure.Commons.Parsers;
using System.Reflection;

namespace Api.DI;

internal static partial class DIextentions {
    static readonly string[] sourceArray = ["Application", "Infrastructure", "Domain", "Api"];

    public static IServiceCollection InjectServices(this IServiceCollection services) {
        var assemblies = sourceArray.Select(Assembly.Load).ToArray();

        return services
            .AddHttpContextAccessor()
            .InjectServices(assemblies)
            .InjectMappers()
            .InjectParsers();
    }

    static IServiceCollection InjectParsers(this IServiceCollection services) {
        return services.AddScoped<IGpxParser, GpxParser>();
    }

    static IServiceCollection InjectMappers(this IServiceCollection services) {
        services.AddScoped<PeakMapper>();
        services.AddScoped<RegionMapper>();
        return services;
    }

    static IServiceCollection InjectServices(
        this IServiceCollection services,
        Assembly[] targetAssemblies
    ) {
        return services.Scan(scan =>
            scan.FromAssemblies(targetAssemblies)
                .AddClasses(
                    classes =>
                        classes.Where(t =>
                            t.Name.EndsWith("Service") && t.IsClass && !t.IsAbstract
                        ),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
    }
}
