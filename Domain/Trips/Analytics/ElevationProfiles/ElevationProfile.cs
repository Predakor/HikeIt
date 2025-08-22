using Domain.Common.Abstractions;
using Domain.Common.Geography;
using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Analytics.ElevationProfiles;

public class ElevationProfile : IEntity<Guid> {
    public Guid Id { get; init; }
    public required GpxPoint Start { get; init; }
    public required byte[] GainsData { get; init; }

    public static ElevationProfile Create(Guid id, GpxPoint start, ICollection<GpxGain> gains) {
        var bytes = ScaledGainSerializer.Serialize([.. gains]);

        return new() {
            Id = id,
            Start = start,
            GainsData = bytes,
        };
    }
}
