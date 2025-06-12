using Domain.Common;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Entities.ElevationProfile;

public class ElevationProfile : IEntity<Guid> {
    public Guid Id { get; init; }
    public required GpxPoint Start { get; init; }
    public required byte[] GainsData { get; init; }

    public static ElevationProfile Create(GpxPoint start, ICollection<GpxGain> gains) {
        var bytes = ScaledGainSerializer.Serialize(gains.ToArray());

        return new() {
            Id = Guid.NewGuid(),
            Start = start,
            GainsData = bytes,
        };
    }
}
