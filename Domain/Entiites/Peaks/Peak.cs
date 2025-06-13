using Domain.Entiites.Regions;
using Domain.Interfaces;

namespace Domain.Entiites.Peaks;

public class Peak : IEntity<int> {
    public int Id { get; set; }
    public int Height { get; set; }

    public string? Name { get; set; }

    public int RegionID { get; set; }
    public Region Region { get; set; } //navigation prop
}
