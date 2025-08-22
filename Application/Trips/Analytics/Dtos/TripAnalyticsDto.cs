using Application.Commons;
using Application.Trips.Analytics.ElevationProfiles;
using Application.Trips.Analytics.PeakAnalytics;
using Domain.Trips.Analytics.Root;
using Domain.Trips.Analytics.Route;
using Domain.Trips.Analytics.Time;

namespace Application.Trips.Analytics.Dtos;

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
