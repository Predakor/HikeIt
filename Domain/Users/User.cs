namespace Domain.Users;

public class User : IEntity {
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDay { get; set; }
}
