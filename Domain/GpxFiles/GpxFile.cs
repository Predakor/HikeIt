using Domain.Users;

namespace Domain.GpxFiles;

public class GpxFile : IEntity {
    public Guid Id { get; set; }

    public required string Name { get; set; }
    public required string Path { get; set; }
    public int? OwnerId { get; set; }

    //navigation prop
    public User? Owner { get; set; }
}
