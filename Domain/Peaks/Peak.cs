using Domain.Common.Factories;
using Domain.Interfaces;
using Domain.Mountains.Regions;
using Domain.Trips.ValueObjects;
using NetTopologySuite.Geometries;

namespace Domain.Peaks;

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
