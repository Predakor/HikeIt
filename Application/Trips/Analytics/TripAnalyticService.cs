using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.Interfaces;
using Application.Trips.Analytics.PeakAnalytics.Commands;
using Application.Trips.Root.ValueObjects;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;
using Domain.ReachedPeaks;
using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.Peaks;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Root.Interfaces;
using Domain.Trips.Analytics.Route.Builders;
using Domain.Trips.Analytics.Time;

namespace Application.Trips.Analytics;

public class TripAnalyticService : ITripAnalyticService {
    readonly ITripAnalyticUnitOfWork _unitOfWork;
    readonly IReachedPeakService _reachedPeakService;
    readonly IElevationProfileService _elevationProfileService;

    public TripAnalyticService(
        ITripAnalyticUnitOfWork unitOfWork,
        IReachedPeakService reachedPeakService,
        IElevationProfileService elevationProfileService
    ) {
        _unitOfWork = unitOfWork;
        _reachedPeakService = reachedPeakService;
        _elevationProfileService = elevationProfileService;
    }

    public async Task<Result<TripAnalytic>> GenerateAnalytic(CreateTripContext ctx) {
        var points = ctx.AnalyticData.Points;
        var gains = ctx.AnalyticData.Gains ?? points.ToGains();

        if (points is null || points.Count == 0) {
            return Errors.EmptyCollection("points");
        }

        var builder = new TripAnalyticBuilder().WithId(ctx.Id);

        GenerateRouteAndTimeAnalytics(points, gains, builder);

        await GeneratePeaksAnalytics(ctx).MapAsync(builder.WithPeaksAnalytic);
        await GenerateElevationProfile(ctx).MapAsync(builder.WithElevationProfile);

        return builder.Build();
    }

    static TripAnalyticBuilder GenerateRouteAndTimeAnalytics(
        List<GpxPoint> points,
        List<GpxGain> gains,
        TripAnalyticBuilder builder
    ) {
        var routeAnalytics = RouteAnalyticFactory.Create(points, gains);
        if (routeAnalytics is null) {
            return builder;
        }

        builder.WithRouteAnalytic(routeAnalytics);

        var timeAnalytics = TimeAnalyticFactory.CreateAnalytics(new(routeAnalytics, points));
        if (timeAnalytics is not null) {
            builder.WithTimeAnalytic(timeAnalytics);
        }

        return builder;
    }

    Task<Result<ElevationProfile>> GenerateElevationProfile(CreateTripContext ctx) {
        return _elevationProfileService.Create(ctx).BindAsync(_unitOfWork.Elevations.AddAsync);
    }

    Task<Result<List<ReachedPeak>>> GenerateReachedPeaks(CreateTripContext ctx) {
        return _reachedPeakService
            .CreateReachedPeaks(ctx.AnalyticData, ctx.Trip)
            .MapAsync(peaks => {
                ctx.Trip.AddReachedPeaks(peaks);
                return peaks;
            });

    }

    Task<Result<PeaksAnalytic>> GeneratePeaksAnalytics(CreateTripContext ctx) {
        return GenerateReachedPeaks(ctx)
            .BindAsync(rp => CreatePeakAnalyticsCommand.Create(ctx.Trip.Id, rp).Execute())
            .BindAsync(_unitOfWork.PeakAnalytics.AddAsync);
    }
}
