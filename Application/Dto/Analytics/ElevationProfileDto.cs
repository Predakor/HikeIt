using Application.TripAnalytics.Commands;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.Trips.ValueObjects;

namespace Application.Dto.Analytics;

public record GainDto(float Dist, float Ele, float Time);

public record ElevationProfileDto(GpxPoint Start, GainDto[] Gains);

public static class ElevationProfileExtentios {
    public static ElevationProfileDto ToDto(this ElevationProfile profile) {
        return DecodeGainsDataCommand
            .Create(profile.GainsData)
            .Execute()
            .Match(gains => new ElevationProfileDto(profile.Start, gains), error => null);
    }
}
