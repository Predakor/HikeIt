using Application.Services.Peaks;
using Application.TripAnalytics.Commands;
using Application.TripAnalytics.Interfaces;
using Domain.Common;
using Domain.Common.Result;
using Domain.Entiites.Users;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Builders.RouteAnalyticsBuilder;
using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.TripAnalytics.Commands;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Factories;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Services;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;
using Domain.Trips;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics;

public class TripAnalyticService(
    IPeakService peakService,
    ITripAnalyticUnitOfWork unitOfWork,
    IReachedPeakService reachedPeakService,
    IPeakAnalyticService peakAnalyticService,
    ITripDomainAnalyticService tripDomainAnalyticService
) : ITripAnalyticService {
    readonly IPeakService _peakService = peakService;
    readonly ITripAnalyticUnitOfWork _unitOfWork = unitOfWork;
    readonly IReachedPeakService _reachedPeakService = reachedPeakService;
    readonly IPeakAnalyticService _peakAnalyticService = peakAnalyticService;
    readonly ITripDomainAnalyticService _tripDomainAnalyticService = tripDomainAnalyticService;

    public async Task<Result<TripAnalytic>> GenerateAnalytic(AnalyticData data, Trip trip, User user) {
        var (points, gains) = data;
        var builder = new TripAnalyticBuilder();

        //generate Route and Time analytics
        GenerateRouteAnalytics(data)
            .Bind(d => {
                builder.WithRouteAnalytic(d);
                return Result<RouteAnalytic>.Success(d);
            })
            .Bind(routeAnalytics => GenerateTimeAnalytics(routeAnalytics, points))
            .Match(timeAnalytics => builder.WithTimeAnalytic(timeAnalytics), err => { });

        //Reached Peaks
        await FindLocalMaximasCommand
            .Create(data)
            .Execute()
            .Bind(localMaximas => _peakService.GetPeaksWithinRadius(localMaximas, 200f))
            .Bind(foundPeaks => _reachedPeakService.ToReachedPeaks(foundPeaks, trip, user))
            .Bind(reachedPeaks => {
                _unitOfWork.ReachedPeaks.AddRangeAsync(reachedPeaks);
                return _peakAnalyticService.Create([.. reachedPeaks]);
            })
            .MatchAsync(
                async peakAnalytics => {
                    await _unitOfWork.PeakAnalytics.AddAsync(peakAnalytics);
                    builder.WithPeaksAnalytic(peakAnalytics);
                },
                error => Console.WriteLine("Couldn't generate peak analytics")
            );

        //Elevation Profile
        await CreateElevationProfileCommand
            .Create(new(data, null))
            .Execute()
            .Bind(_unitOfWork.Elevations.Create)
            .MatchAsync(
                elevationProfile => builder.WithElevationProfile(elevationProfile),
                error => Console.WriteLine(error.Message)
            );

        var entity = builder.Build();
        return entity;
    }

    public Result<RouteAnalytic> GenerateRouteAnalytics(AnalyticData data) {
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

    public Result<TimeAnalytic> GenerateTimeAnalytics(
        RouteAnalytic analytics,
        List<GpxPoint> points
    ) {
        var timeAnalytics = TimeAnalyticFactory.CreateAnalytics(new(analytics, points));
        if (timeAnalytics == null) {
            return Errors.Unknown("Unsuficient data for time anlaytics");
        }

        return timeAnalytics;
    }

    public async Task<ElevationProfile> CreateElevationProfile(
        ElevationDataWithConfig dataWithConfig
    ) {
        var elevationData = GpxDataFactory.Create(dataWithConfig);
        var points = elevationData.Points;

        var profile = ElevationProfile.Create(points.First(), points.ToGains());
        return profile;
    }

    public async Task<TripAnalytic?> GetAnalytic(Guid id) {
        return await _unitOfWork.TripAnalytics.GetByIdAsync(id);
    }

    public async Task<Result<TripAnalytic>> GetCompleteAnalytic(Guid id) {
        var query = await _unitOfWork.TripAnalytics.GetCompleteAnalytic(id);
        return query;
    }

    public async Task<ElevationProfile> GetElevationProfile(Guid id) {
        return (await _unitOfWork.Elevations.GetById(id)).Map(
            data => data,
            error => throw new Exception(error.Message)
        );
    }
}
