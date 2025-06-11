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

    public async Task<TripAnalytic> GenerateAnalytic(TripAnalyticData data) {

        //could be a different step or done before since 
        //its neccesary for analytics creation
        var analyticData = ProccesGpxDataCommand.Run(data);
        if (analyticData.HasErrors(out var error)) {
            throw new Exception(error.Message);
        }
        var (points, gains) = analyticData.Value!;

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

        //Elevation profile wip
        var elevationProfile = points.Where((p, i) => i % 10 == 0).ToList();

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
