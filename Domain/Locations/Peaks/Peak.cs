using Domain.Common.Abstractions;
using Domain.Common.Geography.Factories;
using Domain.Common.Geography.ValueObjects;
using Domain.Locations.Regions;
using NetTopologySuite.Geometries;

namespace Domain.Locations.Peaks;

public class Peak : IEntity<int> {
    public int Id { get; init; }
    public required string Name { get; set; }
    public required int Height { get; set; }
    public required Point Location { get; set; }

    public int RegionID { get; set; }

    //navigation prop
    public Region Region { get; set; } = default!;

    public static Peak Create(string name, IGeoPoint point, int regionId) {
        return new Peak() {
            Name = name,
            Height = (int)point.Ele,
            Location = GeoFactory.CreatePoint(point.Lon, point.Lat),
            RegionID = regionId,
        };
    }

}
