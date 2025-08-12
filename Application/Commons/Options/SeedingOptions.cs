using System.Text.Json;

namespace Application.Commons.Options;

public class SeedingOptions {
    public const string Seeding = "Seeding";
    public string RegionsUrl { get; set; } = string.Empty;
    public string PeaksUrl { get; set; } = string.Empty;
    public UserSeedingOptions Users { get; set; } = new();
}

public class UserSeedingOptions {
    public string Demo { get; set; } = string.Empty;
    public string Admin { get; set; } = string.Empty;

    public UserSeedingData GetAdmin => JsonSerializer.Deserialize<UserSeedingData>(Admin)!;
    public UserSeedingData GetDemo => JsonSerializer.Deserialize<UserSeedingData>(Demo)!;

}

public class UserSeedingData {
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
