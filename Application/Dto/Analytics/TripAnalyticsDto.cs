using Application.Commons;
using Domain.TripAnalytics;
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

    public record Basic(RouteAnalytic Route, TimeAnalytic? Time, Uri? Peaks, Uri? Elevation)
        : TripAnalyticsDto;

    public record Full(
        RouteAnalytic RouteAnalytics,
        TimeAnalytic? TimeAnalytics,
        PeakAnalyticsDto? PeakAnalytics,
        ElevationProfileDto? ElevationProfile,
        Guid Id
    ) : TripAnalyticsDto;
}

public static class Extentions {
    public static TripAnalyticsDto.Basic ToBasicDto(this TripAnalytic analytics) {
        return new(
            analytics.RouteAnalytics,
            analytics.TimeAnalytics,
            analytics.PeaksAnalyticsId.ToResoutceUrl(id => $"trips/{id}/analytics/peaks"),
            analytics.ElevationProfileId.ToResoutceUrl(id => $"trips/{id}/analytics/elevation")
        );
    }
}
