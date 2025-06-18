using Domain.Common;
using Domain.Common.Result;
using Domain.TripAnalytics.Commands;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Services;

public class TripDomainAnalyticsService : ITripDomainAnalyticService {
    public Result<List<GpxPoint>> FindLocalPeaks(List<GpxPoint> points, List<GpxGain> gains) {
        return FindLocalMaximasCommand.Create(new(points, gains)).Execute();
    }

    public List<GpxGain> GenerateGains(List<GpxPoint> points) {
        return points.ToGains();
    }
}
