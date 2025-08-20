using Application.Mappers.Implementations;
using Application.Trips.GpxFile.Services;
using Infrastructure.Parsers;
using System.Reflection;

namespace Api.DI;

internal static partial class DIextentions {
    public static WebApplicationBuilder InjectServices(this WebApplicationBuilder builder) {
        var assemblies = new[] { "Application", "Infrastructure", "Domain", "Api" }
            .Select(Assembly.Load)
            .ToArray();

        builder
            .Services.AddHttpContextAccessor()
            .InjectMappers()
            .InjectServices(assemblies)
            .InjectParsers();

        return builder;
    }

    static IServiceCollection InjectParsers(this IServiceCollection services) {
        services.AddScoped<IGpxParser, GpxParser>();
        return services;
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
        services.Scan(scan =>
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
        return services;
    }
}
