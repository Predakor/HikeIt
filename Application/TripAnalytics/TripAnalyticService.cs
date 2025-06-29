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
        Guid tripId,
        User user
    ) {
        var (points, gains) = data;
        var builder = new TripAnalyticBuilder();

        builder.WithId(tripId);

        //generate Route and Time analytics
        GenerateRouteAnalytics(data)
            .Map(d => {
                builder.WithRouteAnalytic(d);
                return (d);
            })
            .Bind(routeAnalytics => GenerateTimeAnalytics(routeAnalytics, points))
            .Map(timeAnalytics => builder.WithTimeAnalytic(timeAnalytics));

        //Reached Peaks
        await CreatePeakAnalytics(data, tripId, user)
            .MapAsync(peakAnalytics => builder.WithPeaksAnalytic(peakAnalytics));

        //Elevation Profile
        await CreateElevationProfile(data, tripId)
            .MapAsync(elevationProfile => builder.WithElevationProfile(elevationProfile));

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
        Guid tripId,
        User user
    ) {
        return await FindLocalMaximasCommand
            .Create(data)
            .Execute()
            .BindAsync(localMaximas => _peakService.GetPeaksWithinRadius(localMaximas, 200f))
            .BindAsync(foundPeaks => _reachedPeakService.ToReachedPeaks(foundPeaks, tripId, user.Id))
            .BindAsync(_unitOfWork.ReachedPeaks.AddRangeAsync)
            .BindAsync(reachedPeaks =>
                CreatePeakAnalyticsCommand.Create(tripId, new(reachedPeaks, null)).Execute()
            )
            .BindAsync(_unitOfWork.PeakAnalytics.AddAsync);
    }

    public async Task<Result<ElevationProfile>> CreateElevationProfile(AnalyticData data, Guid tripId) {
        return await CreateElevationProfileDataCommand
            .Create(data, GpxDataConfigs.ElevationProfile)
            .Execute()
            .Bind(eleData => CreateElevationProfileCommand.Create(eleData, tripId).Execute())
            .BindAsync(eleProfile => _unitOfWork.Elevations.AddAsync(eleProfile));
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
        return (await _unitOfWork.Elevations.GetById(id)).Match(
            data => data,
            error => throw new Exception(error.Message)
        );
    }
}
