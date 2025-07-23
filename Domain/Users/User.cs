using Domain.Interfaces;
using Domain.Users.Entities;
using Microsoft.AspNetCore.Identity;
namespace Domain.Users;

public class User : IdentityUser<Guid>, IEntity<Guid> {
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly BirthDay { get; set; }
    public string? Avatar { get; set; }

    public UserStats Stats { get; set; }
}
