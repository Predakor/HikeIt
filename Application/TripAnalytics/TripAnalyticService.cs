using Application.Peaks;
using Application.TripAnalytics.Interfaces;
using Application.TripAnalytics.Services;
using Application.Trips;
using Domain.Common;
using Domain.Common.Result;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Builders.RouteAnalyticsBuilder;
using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.TripAnalytics.Commands;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.TripAnalytics.Factories;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;
using Domain.Trips.Config;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics;

public class TripAnalyticService(
    IPeaksQueryService peakRepository,
    ITripAnalyticUnitOfWork unitOfWork,
    IReachedPeakService reachedPeakService
) : ITripAnalyticService {
    readonly IPeaksQueryService _peakRepository = peakRepository;
    readonly ITripAnalyticUnitOfWork _unitOfWork = unitOfWork;
    readonly IReachedPeakService _reachedPeakService = reachedPeakService;

    static readonly float PeakProximityTreshold = 1000f;

    public async Task<Result<TripAnalytic>> GenerateAnalytic(CreateTripContext ctx) {
        var (points, gains) = ctx.AnalyticData;

        var builder = new TripAnalyticBuilder().WithId(ctx.Id);

        //generate Route and Time analytics
        GenerateRouteAnalytics(ctx.AnalyticData)
            .Map(d => {
                builder.WithRouteAnalytic(d);
                return (d);
            })
            .Bind(routeAnalytics => GenerateTimeAnalytics(routeAnalytics, points))
            .Map(builder.WithTimeAnalytic);

        await CreatePeakAnalytics(ctx).MapAsync(builder.WithPeaksAnalytic);
        await CreateElevationProfile(ctx).MapAsync(builder.WithElevationProfile);

        return builder.Build();
    }

    Result<RouteAnalytic> GenerateRouteAnalytics(AnalyticData data) {
        var (points, gains) = data;
        if (points == null) {
            return Errors.EmptyCollection("points");
        }
        if (gains == null) {
            return Errors.EmptyCollection("gains");
        }

        var routeAnalytic = RouteAnalyticFactory.Create(points, gains);
        if (routeAnalytic == null) {
            return Errors.Unknown("something went wrong while generatin route anlaytics");
        }

        return routeAnalytic;
    }

    Result<TimeAnalytic> GenerateTimeAnalytics(RouteAnalytic analytics, List<GpxPoint> points) {
        var timeAnalytics = TimeAnalyticFactory.CreateAnalytics(new(analytics, points));
        if (timeAnalytics == null) {
            return Errors.Unknown("insuficient data for time anlaytics");
        }

        return timeAnalytics;
    }

    async Task<Result<PeaksAnalytic>> CreatePeakAnalytics(CreateTripContext ctx) {
        var potentialPeaks = FindLocalMaximasCommand.Create(ctx.AnalyticData).Execute();

        return await potentialPeaks
            .BindAsync(localMaximas =>
                _peakRepository.GetPeaksWithinRadius(localMaximas, PeakProximityTreshold)
            )
            .BindAsync(foundPeaks =>
                _reachedPeakService.ToReachedPeaks(foundPeaks, ctx.Trip.Id, ctx.User.Id)
            )
            .BindAsync(_unitOfWork.ReachedPeaks.AddRangeAsync)
            .BindAsync(reachedPeaks =>
                CreatePeakAnalyticsCommand.Create(ctx.Trip.Id, new(reachedPeaks, null)).Execute()
            )
            .BindAsync(_unitOfWork.PeakAnalytics.AddAsync);
    }

    async Task<Result<ElevationProfile>> CreateElevationProfile(CreateTripContext ctx) {
        var elevationData = CreateElevationProfileDataCommand
            .Create(ctx.AnalyticData, GpxDataConfigs.ElevationProfile)
            .Execute();

        return await elevationData
            .Bind(eleData => CreateElevationProfileCommand.Create(eleData, ctx.Trip.Id).Execute())
            .BindAsync(_unitOfWork.Elevations.AddAsync);
    }
}
