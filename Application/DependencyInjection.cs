using Application.AppSettings;
using Application.AppSettings.Decorators;
using Application.Commons.Abstractions;
using Application.Commons.Drafts;
using Application.Trips.Analytics.Pipeline;
using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        return services
            .AddDomainEvents()
            .AddDrafts()
            .AddPipeline()
            .DecorateAppSettings();
    }

    // PipelinePreviewService is picked up by InjectServices (name ends with "Service"), so only the source and stages are listed here.
    private static IServiceCollection AddPipeline(this IServiceCollection services)
    {
        return services
            .AddScoped<PreviewCache>()
            .AddScoped<IPipelineSource, TripFilePipelineSource>()
            .AddScoped<IPointsFilterRunner, PointsFilterRunner>()
            .AddScoped<IPeaksFilterRunner, PeaksFilterRunner>()
            .AddScoped<ITerrainRunner, TerrainRunner>()
            .AddScoped<IPaceRunner, PaceRunner>()
            .Scan(scan =>
                scan.FromAssembliesOf(typeof(DependencyInjection))
                    .AddClasses(
                        classes => classes.AssignableTo<IPipelineStage>(),
                        publicOnly: false
                    )
                    .As<IPipelineStage>()
                    .WithScopedLifetime()
            );
    }

    private static IServiceCollection AddDomainEvents(this IServiceCollection services)
    {
        return services.Scan(scan =>
            scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(
                    classes => classes.AssignableTo(typeof(IDomainEventHandler<>)),
                    publicOnly: false
                )
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
    }

    private static IServiceCollection AddDrafts(this IServiceCollection services)
    {
        return services.AddSingleton(typeof(IDraftService<>), typeof(MemoryDraftService<>));
    }

    private static IServiceCollection DecorateAppSettings(this IServiceCollection services)
    {
        return services.Decorate<IAppSettingsService, CachedAppSettingsServiceDecorator>();
    }

}