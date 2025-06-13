using Domain.Interfaces;

namespace Domain.Entiites.Users;

public class User : IEntity<Guid> {
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDay { get; set; }
}
