using Application.Dto;
using Application.Services.Peaks;
using Application.TripAnalytics.Interfaces;
using Application.TripAnalytics.Services;
using Application.Trips;
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

    public async Task<Result<TripAnalytic>> GetAnalytic(Guid id) {
        var queryResult = await _unitOfWork.TripAnalytics.GetByIdAsync(id);
        return queryResult != null ? queryResult : Errors.NotFound("Analytics with id:" + id);
    }

    public async Task<Result<TripAnalytic>> GenerateAnalytic(CreateTripContext ctx) {
        var (points, gains) = ctx.AnalyticData;

        var builder = new TripAnalyticBuilder();
        builder.WithId(ctx.Id);

        //generate Route and Time analytics
        GenerateRouteAnalytics(ctx.AnalyticData)
            .Map(d => {
                builder.WithRouteAnalytic(d);
                return (d);
            })
            .Bind(routeAnalytics => GenerateTimeAnalytics(routeAnalytics, points))
            .Map(builder.WithTimeAnalytic);

        //Reached Peaks
        await CreatePeakAnalytics(ctx.AnalyticData, ctx.Id, ctx.User)
            .MapAsync(builder.WithPeaksAnalytic);

        //Elevation Profile
        await CreateElevationProfile(ctx.AnalyticData, ctx.Id)
            .MapAsync(builder.WithElevationProfile);

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

    public async Task<Result<PeaksAnalytic>> CreatePeakAnalytics(
        AnalyticData data,
        Guid tripId,
        User user
    ) {
        return await FindLocalMaximasCommand
            .Create(data)
            .Execute()
            .BindAsync(localMaximas => {
                Console.WriteLine(localMaximas);
                return _peakService.GetPeaksWithinRadius(localMaximas, 2000f);
            })
            .BindAsync(foundPeaks =>
                _reachedPeakService.ToReachedPeaks(foundPeaks, tripId, user.Id)
            )
            .BindAsync(_unitOfWork.ReachedPeaks.AddRangeAsync)
            .BindAsync(reachedPeaks =>
                CreatePeakAnalyticsCommand.Create(tripId, new(reachedPeaks, null)).Execute()
            )
            .BindAsync(_unitOfWork.PeakAnalytics.AddAsync);
    }

    public async Task<Result<ElevationProfile>> CreateElevationProfile(
        AnalyticData data,
        Guid tripId
    ) {
        return await CreateElevationProfileDataCommand
            .Create(data, GpxDataConfigs.ElevationProfile)
            .Execute()
            .Bind(eleData => CreateElevationProfileCommand.Create(eleData, tripId).Execute())
            .BindAsync(_unitOfWork.Elevations.AddAsync);
    }

    public async Task<Result<Full>> GetCompleteAnalytic(Guid id) {
        return await _unitOfWork
            .TripAnalytics.GetCompleteAnalytic(id)
            .MapAsync(AnalyticsServiceHelpers.MapToDto);
    }

    public async Task<ElevationProfile> GetElevationProfile(Guid id) {
        return (await _unitOfWork.Elevations.GetById(id)).Match(
            data => data,
            error => throw new Exception(error.Message)
        );
    }
}

internal static class AnalyticsServiceHelpers {
    public static Full MapToDto(TripAnalytic data) {
        var elevationDto = data.ElevationProfile?.ToDto();
        var peakAnalyticDto = data.PeaksAnalytic?.ToDto();
        var timeAnalyticDto = data.TimeAnalytics ?? null;
        var routeAnalyticsDto = data.RouteAnalytics ?? null;

        return new Full(routeAnalyticsDto, timeAnalyticDto, peakAnalyticDto, elevationDto, data.Id);
    }
}
