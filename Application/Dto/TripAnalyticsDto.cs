using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;

namespace Application.Dto;

public abstract record TripAnalyticsDto(Guid Id) {
    public record Full(
        RouteAnalytic RouteAnalytics,
        TimeAnalytic? TimeAnalytics,
        PeakAnalyticsDto? PeakAnalytics,
        ElevationProfileDto? ElevationProfile,
        Guid Id
    ) : TripAnalyticsDto(Id);
}
