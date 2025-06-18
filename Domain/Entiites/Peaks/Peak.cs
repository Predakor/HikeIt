using Domain.Entiites.Regions;
using Domain.Interfaces;
using NetTopologySuite.Geometries;

namespace Domain.Entiites.Peaks;

public class Peak : IEntity<int> {
    public int Id { get; set; }
    public required int Height { get; set; }
    public required Point Location { get; set; }

    public string? Name { get; set; }

    public int RegionID { get; set; }
    public Region Region { get; set; } //navigation prop
}
