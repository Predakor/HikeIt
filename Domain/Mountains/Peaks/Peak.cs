using Domain.Interfaces;
using Domain.Mountains.Regions;
using NetTopologySuite.Geometries;

namespace Domain.Mountains.Peaks;

public class Peak : IEntity<int> {
    public int Id { get; init; }
    public required int Height { get; set; }
    public required Point Location { get; set; }
    public required string Name { get; set; }

    public int RegionID { get; set; }
    public Region Region { get; set; } //navigation prop
}
