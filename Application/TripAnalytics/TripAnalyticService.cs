using Application.Dto;
using Application.Services.Peaks;
using Application.TripAnalytics.Interfaces;
using Application.TripAnalytics.Services;
using Domain.Common;
using Domain.Common.Result;
using Domain.Entiites.Users;
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
using Domain.Trips;
using Domain.Trips.Config;
using Domain.Trips.ValueObjects;
using static Application.Dto.TripAnalyticsDto;

namespace Application.TripAnalytics;

public class TripAnalyticService(
    IPeakService peakService,
    ITripAnalyticUnitOfWork unitOfWork,
    IReachedPeakService reachedPeakService
) : ITripAnalyticService {
    readonly IPeakService _peakService = peakService;
    readonly ITripAnalyticUnitOfWork _unitOfWork = unitOfWork;
    readonly IReachedPeakService _reachedPeakService = reachedPeakService;

    public async Task<Result<TripAnalytic>> GenerateAnalytic(
        AnalyticData data,
        Trip trip,
        User user
    ) {
        var (points, gains) = data;
        var builder = new TripAnalyticBuilder();

        builder.WithId(trip.Id);

        //generate Route and Time analytics
        GenerateRouteAnalytics(data)
            .Bind(d => {
                builder.WithRouteAnalytic(d);
                return Result<RouteAnalytic>.Success(d);
            })
            .Bind(routeAnalytics => GenerateTimeAnalytics(routeAnalytics, points))
            .Match(timeAnalytics => builder.WithTimeAnalytic(timeAnalytics), err => { });

        //Reached Peaks
        await CreatePeakAnalytics(data, trip, user)
            .MatchAsync(
                peakAnalytics => builder.WithPeaksAnalytic(peakAnalytics),
                error => Console.WriteLine("Couldn't generate peak analytics")
            );

        //Elevation Profile
        await CreateElevationProfile(data, trip)
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

    public async Task<TripAnalytic?> GetAnalytic(Guid id) {
        return await _unitOfWork.TripAnalytics.GetByIdAsync(id);
    }

    public async Task<Result<PeaksAnalytic>> CreatePeakAnalytics(
        AnalyticData data,
        Trip trip,
        User user
    ) {
        return await FindLocalMaximasCommand
            .Create(data)
            .Execute()
            .Bind(localMaximas => _peakService.GetPeaksWithinRadius(localMaximas, 200f))
            .Bind(foundPeaks => _reachedPeakService.ToReachedPeaks(foundPeaks, trip, user))
            .Bind(_unitOfWork.ReachedPeaks.AddRangeAsync)
            .Bind(reachedPeaks =>
                CreatePeakAnalyticsCommand.Create(trip.Id, new(reachedPeaks, null)).Execute()
            )
            .Bind(_unitOfWork.PeakAnalytics.AddAsync);
    }

    public async Task<Result<ElevationProfile>> CreateElevationProfile(AnalyticData data, Trip trip) {
        return await CreateElevationProfileDataCommand
            .Create(data, GpxDataConfigs.ElevationProfile)
            .Execute()
            .Bind(eleData => CreateElevationProfileCommand.Create(eleData, trip.Id).Execute())
            .Bind(eleProfile => _unitOfWork.Elevations.AddAsync(eleProfile));
    }

    public async Task<Result<Full>> GetCompleteAnalytic(Guid id) {
        var query = await _unitOfWork.TripAnalytics.GetCompleteAnalytic(id);

        if (query.HasErrors(out var error)) {
            return Errors.Unknown(error.Message);
        }

        var result = query.Value;
        if (result == null) {
            return Errors.NotFound("result");
        }

        var gains = result.ElevationProfile?.GainsData;
        if (gains == null) {
            return Errors.NotFound("No gains");
        }

        var elevationDto = result.ElevationProfile?.ToDto();
        var peakAnalyticDto = result.PeaksAnalytic?.ToDto();

        var analytics = new Full(
            result.RouteAnalytics ?? null,
            result.TimeAnalytics ?? null,
            peakAnalyticDto,
            elevationDto,
            result.Id
        );

        return analytics;
    }

    public async Task<ElevationProfile> GetElevationProfile(Guid id) {
        return (await _unitOfWork.Elevations.GetById(id)).Map(
            data => data,
            error => throw new Exception(error.Message)
        );
    }
}
