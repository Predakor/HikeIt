using Domain.Interfaces;
using Domain.Users.Entities;
using Domain.Users.RegionProgres;
using Domain.Users.RegionProgresses.Factories;
using Domain.Users.RegionProgresses.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Users;

public class User : IdentityUser<Guid>, IEntity<Guid> {
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly BirthDay { get; set; }
    public string? Avatar { get; set; }

    public UserStats Stats { get; private set; } = new();
    public List<RegionProgress> RegionProgresses { get; private set; } = [];

    public User UpdateOrAddRegionProgress(UpdateRegionProgress progressUpdate) {
        var regionToUpdate = RegionProgresses.FirstOrDefault(rp =>
            rp.RegionId == progressUpdate.RegionId
        );

        if (regionToUpdate is null) {
            RegionProgresses.Add(RegionProgressFactory.FromProgressUpdate(progressUpdate, Id));
            return this;
        }

        regionToUpdate.AddPeakVisits(progressUpdate.PeaksIds);
        return this;
    }

    public static readonly User DemoUser = new() {
        Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
        UserName = "defaultuser",
        FirstName = "Default",
        LastName = "User",
        Email = "default@gmail.com",
        EmailConfirmed = true,
    };
}
