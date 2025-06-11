using Application.Services.Peaks;
using Domain.Common;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Builders.RouteAnalyticsBuilder;
using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.TripAnalytics.Factories;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Services;
using Domain.TripAnalytics.ValueObjects.PeaksAnalytics;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.ValueObjects;

namespace Application.Services;

public class TripAnalyticService(
    IPeakService peakService,
    ITripDomainAnalyticService tripDomainAnalyticService,
    ITripAnalyticRepository tripAnalyticRepository
) : ITripAnalyticService {
    readonly IPeakService _peakRepository = peakService;
    readonly ITripDomainAnalyticService _tripDomainAnalyticService = tripDomainAnalyticService;
    readonly ITripAnalyticRepository _tripAnalyticRepository = tripAnalyticRepository;

    public async Task<TripAnalytic> GenerateAnalytic(TripAnalyticData data) {
        var points = GpxDataBuilder.ProcessData(data).Data;
        var gains = points.ToGains();

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

        if (reachedPeaks.Count > 0) {
            List<ReachedPeak> peaks = reachedPeaks
                .Select(p => ReachedPeak.Create(tripId, userId, p.Id))
                .ToList();

            var highestPeak = ReachedPeak.Create(
                tripId,
                userId,
                reachedPeaks.MaxBy(p => p.Height).Id
            );

            var peakAnalytics = PeaksAnalytic.Create(peaks);


            //add latter
            //builder.WithPeaksAnalytic(peakAnalytics);
        }

        //Elevation profile wip
        var elevationProfile = points
            .Where((p, i) => i % 10 == 0)
            .ToList();

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
