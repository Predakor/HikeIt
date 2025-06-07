using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Services;

public interface ITripDomainAnalyticService {
    public List<GpxPoint> FindLocalPeaks(List<GpxPoint> points, List<GpxGain> gains);
    public List<GpxGain> GenerateGains(List<GpxPoint> points);

}