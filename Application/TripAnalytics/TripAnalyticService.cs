using Application.TripAnalytics.ElevationProfiles;
using Application.TripAnalytics.Interfaces;
using Application.TripAnalytics.PeakAnalytics.Commands;
using Application.Trips;
using Domain.Common;
using Domain.Common.Result;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Builders.RouteAnalyticsBuilder;
using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.TripAnalytics.Factories;
using Domain.TripAnalytics.Interfaces;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics;

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

    void GenerateRouteAndTimeAnalytics(
        List<GpxPoint> points,
        List<GpxGain> gains,
        TripAnalyticBuilder builder
    ) {
        var routeAnalytics = RouteAnalyticFactory.Create(points, gains);
        if (routeAnalytics is null) {
            return;
        }

        builder.WithRouteAnalytic(routeAnalytics);

        var timeAnalytics = TimeAnalyticFactory.CreateAnalytics(new(routeAnalytics, points));
        if (timeAnalytics is not null) {
            builder.WithTimeAnalytic(timeAnalytics);
        }
    }

    Task<Result<ElevationProfile>> GenerateElevationProfile(CreateTripContext ctx) {
        return _elevationProfileService.Create(ctx).BindAsync(_unitOfWork.Elevations.AddAsync);
    }

    Task<Result<IList<ReachedPeak>>> GenerateReachedPeaks(CreateTripContext ctx) {
        return _reachedPeakService
            .CreateReachedPeaks(ctx.AnalyticData, ctx.Trip)
            .BindAsync(_unitOfWork.ReachedPeaks.AddRangeAsync);
    }

    Task<Result<PeaksAnalytic>> GeneratePeaksAnalytics(CreateTripContext ctx) {
        return GenerateReachedPeaks(ctx)
            .BindAsync(rp => CreatePeakAnalyticsCommand.Create(ctx.Trip.Id, rp).Execute())
            .BindAsync(_unitOfWork.PeakAnalytics.AddAsync);
    }
}
