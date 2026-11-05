using Application.Trips.Analytics.Pipeline.Contracts;
using Domain.Common.Geography.Extentions;
using Domain.Trips.Analytics.Route.Builders;
using Domain.Trips.Analytics.Time;

namespace Application.Trips.Analytics.Pipeline.Stages;

// Route and time analytics of the cleaned points, built the same way TripAnalyticService builds them.
public sealed class AnalyticsStage : IPipelineStage
{
    public StageId Id => StageId.Analytics;

    // Has no config of its own; it only depends on the cleanup output.
    public object? SelectConfig(PipelineConfig config) => null;

    public Task<Result<StageOutput>> RunAsync(
        StageContext context,
        PipelineConfig config,
        CancellationToken cancellationToken
    )
    {
        var cleaned = CleanupStage.CleanedDataFrom(context);
        var points = cleaned.Points;
        var gains = cleaned.Gains ?? points.ToGains();

        var route = RouteAnalyticFactory.Create(points, gains);
        var time = TimeAnalyticFactory.CreateAnalytics(new(route, points));

        return Task.FromResult<Result<StageOutput>>(
            new StageOutput(
                new StageData.Analytics(route, time),
                points.Count)
            );
    }
}