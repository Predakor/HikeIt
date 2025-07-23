using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace Domain.Users;

public class User : IdentityUser<Guid>, IEntity<Guid> {
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly BirthDay { get; set; }
    public string? Avatar { get; set; }



    public static readonly User DemoUser = new() {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        UserName = "defaultuser",
        FirstName = "Default",
        LastName = "User",
        Email = "default@gmail.com",
        EmailConfirmed = true,
    };
}
