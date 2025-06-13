using Domain.Entiites.Users;
using Domain.Interfaces;

namespace Domain.Trips.Entities.GpxFiles;

public class GpxFile : IEntity<Guid> {
    public Guid Id { get; set; }

    public required string Name { get; set; }
    public required string Path { get; set; }
    public Guid? OwnerId { get; set; }

    //navigation prop
    public User? Owner { get; set; }
}
