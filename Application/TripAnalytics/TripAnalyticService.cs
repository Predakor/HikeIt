using Application.Services.Peaks;
using Application.TripAnalytics.Commands;
using Application.TripAnalytics.Interfaces;
using Application.TripAnalytics.Services;
using Domain.Common;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Builders.RouteAnalyticsBuilder;
using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Factories;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Services;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics;

public class TripAnalyticService(
    IPeakService peakService,
    ITripDomainAnalyticService tripDomainAnalyticService,
    IElevationProfileService elevationProfileService,
    ITripAnalyticUnitOfWork unitOfWork
) : ITripAnalyticService {
    readonly IPeakService _peakService = peakService;
    readonly ITripDomainAnalyticService _tripDomainAnalyticService = tripDomainAnalyticService;
    readonly IElevationProfileService _elevationService = elevationProfileService;
    readonly ITripAnalyticUnitOfWork unitOfWork = unitOfWork;

    public async Task<TripAnalytic> GenerateAnalytic(AnalyticData data) {
        var (points, gains) = data;

        var builder = new TripAnalyticBuilder(_elevationService);

        //generate Route analytics
        var routeAnalytic = RouteAnalyticFactory.Create(points, gains);
        builder.WithRouteAnalytic(routeAnalytic);

        //Time analytics might be null
        var timeAnalytics = TimeAnalyticFactory.CreateAnalytics(new(routeAnalytic, points));
        if (timeAnalytics != null) {
            builder.WithTimeAnalytic(timeAnalytics);
        }

        //peak detection semi done

        var tripId = Guid.NewGuid(); //Add actual value
        var userId = Guid.NewGuid(); //Add actual value

        //var potentialPeaks = _tripDomainAnalyticService.FindLocalPeaks(points, gains);
        //var reachedPeaks = await _peakService.GetMatchingPeaks(potentialPeaks);
        //Create reached peaks
        //CreateReachedPeaksCommand
        //    .Create(new(reachedPeaks, tripId, userId))
        //    .Execute()
        //    .Match(
        //        async peaks => {
        //            var query = await unitOfWork.ReachedPeaks.AddRangeAsync(peaks);
        //            query.Match(
        //                data => {
        //                    var analytics = PeaksAnalytic.Create(data);
        //                    //unitOfWork.
        //                    builder.WithPeaksAnalytic(PeaksAnalytic.Create(data));
        //                },
        //                error => { }
        //            );
        //        },
        //        error => Console.WriteLine($"Failed to create peak analytics: {error.Message}")
        //    );

        //Elevation implement actual downsampling
        CreateElevationProfileCommand
            .Create(new(data, null))
            .Execute()
            .Match(
                async eleProfile => {
                    var querry = await unitOfWork.Elevations.Create(eleProfile);
                    querry.Match(
                        data => builder.WithElevationProfile(data),
                        error => Console.WriteLine(error.Message)
                    );
                },
                error => Console.WriteLine(error.Message)
            );

        var entity = builder.Build();

        var result = await unitOfWork.TripAnalytics.AddAsync(entity);

        return entity;
    }

    public async Task<TripAnalytic?> GetAnalytic(Guid id) {
        return await unitOfWork.TripAnalytics.GetByIdAsync(id);
    }

    public async Task<Result<TripAnalytic>> GetCompleteAnalytic(Guid id) {
        var query = await unitOfWork.TripAnalytics.GetCompleteAnalytic(id);
        return query;
    }

    public async Task<ElevationProfile> GetElevationProfile(Guid id) {
        return (await unitOfWork.Elevations.GetById(id)).Map(
            data => data,
            error => throw new Exception(error.Message)
        );
    }
}
