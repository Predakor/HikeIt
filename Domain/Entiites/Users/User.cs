using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace Domain.Entiites.Users;

public class User : IdentityUser<Guid>, IEntity<Guid> {
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public DateOnly BirthDay { get; set; }
}
