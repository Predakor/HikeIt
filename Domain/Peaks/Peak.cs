using Domain.Regions;

namespace Domain.Peaks;

public class Peak : IEntity {
    public int Id { get; set; }
    public int Height { get; set; }

    public string? Name { get; set; }
    public int RegionID { get; set; }


    public Region Region { get; set; } //navigation prop
}
