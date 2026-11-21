using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Interfaces;
using Application.Trips.Analytics.PeakAnalytics.Commands;
using Application.Trips.Analytics.Pipeline.Runners;
using Application.Trips.Analytics.RouteVisualizations;
using Application.Trips.Root.ValueObjects;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.ReachedPeaks;
using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.Peaks;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Root.Interfaces;
using Domain.Trips.Analytics.Route;
using Domain.Trips.Analytics.Time;

namespace Application.Trips.Analytics;

public class TripAnalyticService : ITripAnalyticService
{
    private readonly ITripAnalyticUnitOfWork _unitOfWork;
    private readonly IReachedPeakService _reachedPeakService;
    private readonly IElevationProfileService _elevationProfileService;
    private readonly IRouteVisualizationService _routeVisualizations;
    private readonly ITerrainRunner _terrain;
    private readonly IPaceRunner _pace;

    public TripAnalyticService(
        ITripAnalyticUnitOfWork unitOfWork,
        IReachedPeakService reachedPeakService,
        IElevationProfileService elevationProfileService,
        IRouteVisualizationService routeVisualizations,
        ITerrainRunner terrain,
        IPaceRunner pace
    )
    {
        _unitOfWork = unitOfWork;
        _reachedPeakService = reachedPeakService;
        _elevationProfileService = elevationProfileService;
        _routeVisualizations = routeVisualizations;
        _terrain = terrain;
        _pace = pace;
    }

    public async Task<Result<TripAnalytic>> GenerateAnalytic(CreateTripContext ctx)
    {
        var points = ctx.AnalyticData.Points;
        var gains = ctx.AnalyticData.Gains ?? points.ToGains();

        if (points is null || points.Count == 0)
        {
            return Errors.EmptyCollection("points");
        }

        var builder = new TripAnalyticBuilder().WithId(ctx.Id);

        await GenerateRouteAnalytics(points, gains)
            .TapAsync(route => builder.WithRouteAnalytic(route))
            .BindAsync(route => GenerateTimeAnalytics(route, points))
            .TapAsync(time => builder.WithTimeAnalytic(time));

        await GeneratePeaksAnalytics(ctx).TapAsync(t => builder.WithPeaksAnalytic(t));
        await GenerateElevationProfile(ctx).TapAsync(t => builder.WithElevationProfile(t));
        await GenerateRouteVisualization(points).TapAsync(path => builder.WithVisualisation(path));

        return builder.Build();
    }

    private Task<Result<RouteAnalytic>> GenerateRouteAnalytics(List<GpxPoint> points, List<GpxGain> gains)
    {
        return _terrain.ApplyAsync(points, gains);
    }

    private Task<Result<TimeAnalytic>> GenerateTimeAnalytics(RouteAnalytic route, List<GpxPoint> points)
    {
        return _pace.ApplyAsync(route, points);
    }

    private Task<Result<RoutePath>> GenerateRouteVisualization(List<GpxPoint> points)
    {
        return _routeVisualizations.GetRouteVisualizationAsync(points);
    }

    private Task<Result<ElevationProfile>> GenerateElevationProfile(CreateTripContext ctx)
    {
        return _elevationProfileService.Create(ctx).BindAsync(_unitOfWork.Elevations.AddAsync);
    }

    private Task<Result<List<ReachedPeak>>> GenerateReachedPeaks(CreateTripContext ctx)
    {
        return _reachedPeakService
            .CreateReachedPeaks(ctx.AnalyticData, ctx.Trip)
            .TapAsync(peaks => ctx.Trip.AddReachedPeaks(peaks));
    }

    private Task<Result<PeaksAnalytic>> GeneratePeaksAnalytics(CreateTripContext ctx)
    {
        return GenerateReachedPeaks(ctx)
            .BindAsync(rp => CreatePeakAnalyticsCommand.Create(ctx.Trip.Id, rp).Execute())
            .BindAsync(_unitOfWork.PeakAnalytics.AddAsync);
    }
}