using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;

namespace Application.Dto.Analytics;

public record AnalyticsPresence(Uri? HasPeakAnalytics, Uri? HasElevationProfile);

public abstract record TripAnalyticsDto {
    public record Linked(
        Uri? RouteAnalytics,
        Uri? TimeAnalytics,
        Uri? PeakAnalytics,
        Uri? ElevationProfile
    ) : TripAnalyticsDto;

    public record Basic(
        RouteAnalytic RouteAnalytics,
        TimeAnalytic? TimeAnalytics,
        Uri? Peaks,
        Uri? ElevationProfile
    ) : TripAnalyticsDto;

    public record Full(
        RouteAnalytic RouteAnalytics,
        TimeAnalytic? TimeAnalytics,
        PeakAnalyticsDto? PeakAnalytics,
        ElevationProfileDto? ElevationProfile,
        Guid Id
    ) : TripAnalyticsDto;
}
