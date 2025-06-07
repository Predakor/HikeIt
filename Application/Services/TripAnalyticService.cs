using Application.Services.Peaks;
using Domain.ReachedPeaks;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Builders.RouteAnalyticBuilder;
using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.TripAnalytics.Builders.TripTimeAnalyticBuilder;
using Domain.TripAnalytics.Services;
using Domain.TripAnalytics.ValueObjects.PeaksAnalytics;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.ValueObjects;

namespace Application.Services;

public class TripAnalyticService(
    IPeakService peakService,
    ITripDomainAnalyticService tripDomainAnalyticService
) : ITripAnalyticService {
    readonly IPeakService _peakRepository = peakService;
    readonly ITripDomainAnalyticService _tripDomainAnalyticService = tripDomainAnalyticService;

    public async Task<TripAnalytic> GenerateAnalytic(TripAnalyticData data) {
        var points = GpxDataBuilder.ProcessData(data).Data;

        var builder = new TripAnalyticBuilder();
        var gains = _tripDomainAnalyticService.GenerateGains(data.Data);

        //generate Route analytics
        RouteAnalytic routeAnalytic = new RouteAnalyticsBuilder(points, gains)
            .WithTotalDistance()
            .WithTotalAscent()
            .WithTotalDescent()
            .WithHighestPoint()
            .WithLowestPoint()
            .WithAverageSlope()
            .WithAverageDescentSlope()
            .WithAverageAscentSlope()
            .Build();

        builder.WithRouteAnalytic(routeAnalytic);

        //Time analytics might be null
        List<GpxGainWithTime> gainsWithTime = gains.MapToTimed();
        if (gainsWithTime.Count != 0) {
            DateTime start = (DateTime)points.First().Time!;
            DateTime end = (DateTime)points.Last().Time!;

            TimeFrame tripTimeFrame = new(start, end);
            TimeAnalyticData timeAnalyticsData = new(routeAnalytic, gainsWithTime, tripTimeFrame);

            var analytics = TimeAnalyticsDirector.Create(timeAnalyticsData);

            if (analytics != null) {
                builder.WithTimeAnalytic(analytics);
            }
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

            var peakAnalytics = new PeaksAnalytic() { Peaks = peaks, Highest = highestPeak };

            builder.WithPeaksAnalytic(peakAnalytics);
        }



        //RouteAnalytics done
        //TimeAnalytics  done
        //PeaksAnalytics not done
        //ElevationProfile not done

        throw new NotImplementedException();
    }
}
