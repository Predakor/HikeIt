using Application.Services.Peaks;
using Application.TripAnalytics.Commands;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Builders.RouteAnalyticsBuilder;
using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.TripAnalytics.Factories;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Services;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics;

public class TripAnalyticService(
    IPeakService peakService,
    ITripDomainAnalyticService tripDomainAnalyticService,
    ITripAnalyticRepository tripAnalyticRepository
) : ITripAnalyticService {
    readonly IPeakService _peakRepository = peakService;
    readonly ITripDomainAnalyticService _tripDomainAnalyticService = tripDomainAnalyticService;
    readonly ITripAnalyticRepository _tripAnalyticRepository = tripAnalyticRepository;

    public async Task<TripAnalytic> GenerateAnalytic(AnalyticData data) {
        var (points, gains) = data;

        var builder = new TripAnalyticBuilder();

        //generate Route analytics
        var routeAnalytic = RouteAnalyticFactory.Create(points, gains);
        builder.WithRouteAnalytic(routeAnalytic);

        //Time analytics might be null
        var timeAnalytics = TimeAnalyticFactory.CreateAnalytics(new(routeAnalytic, points));
        if (timeAnalytics != null) {
            builder.WithTimeAnalytic(timeAnalytics);
        }

        //peak detection semi done
        var potentialPeaks = _tripDomainAnalyticService.FindLocalPeaks(points, gains);
        var reachedPeaks = await _peakRepository.GetMatchingPeaks(potentialPeaks);

        var tripId = Guid.NewGuid(); //Add actual value
        var userId = Guid.NewGuid(); //Add actual value

        CreatePeakAnalyticsCommand
            .Create(new(reachedPeaks, tripId, userId))
            .Execute()
            .Match(
                data => builder.WithPeaksAnalytic(data),
                error => Console.WriteLine($"couldn't create peak analytics reason{error.Message}")
            );

        //Elevation implement actual downsampling
        CreateElevationProfileCommand
            .Create(new(data, null))
            .Execute()
            .Match(
                data => builder.WithElevationProfile(data),
                error => Console.WriteLine(error.Message)
            );

        //PeaksAnalytics not done
        //ElevationProfile not done

        var entity = builder.Build();

        var result = await _tripAnalyticRepository.AddAsync(entity);

        if (result == false) {
            throw new Exception();
        }

        return entity;
    }

    public async Task<TripAnalytic?> GetAnalytic(Guid id) {
        return await _tripAnalyticRepository.GetByIdAsync(id);
    }
}
