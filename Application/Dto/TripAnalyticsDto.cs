using Domain.TripAnalytics.Entities.PeaksAnalytics;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;
using Domain.Trips.ValueObjects;

namespace Application.Dto;

public abstract record TripAnalyticsDto(Guid Id) {
    public record Full(
        RouteAnalytic RouteAnalytics,
        TimeAnalytic? TimeAnalytics,
        PeaksAnalytic? PeaksAnalytic,
        ElevationProfileDto? ElevationProfile,
        Guid Id
    ) : TripAnalyticsDto(Id);



    public record GainDto(float Dist, float Ele, float Time);

    public record ElevationProfileDto(GpxPoint Start, GainDto[] Gains);
}
