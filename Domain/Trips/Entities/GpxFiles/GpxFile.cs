using Domain.Interfaces;

namespace Domain.Trips.Entities.GpxFiles;

public class GpxFile : IEntity<Guid> {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Path { get; set; }
    public required string OriginalName { get; set; }
    public required DateTime CreatedAt { get; set; }
}
