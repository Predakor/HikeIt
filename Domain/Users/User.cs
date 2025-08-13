using Domain.Interfaces;
using Domain.Ranks;
using Domain.Trips;
using Domain.Users.Entities;
using Domain.Users.RegionProgresses;
using Domain.Users.RegionProgresses.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Users;


public enum Gender {
    Male,
    Female,
    Other
}

public class User : IdentityUser<Guid>, IEntity<Guid> {
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly BirthDay { get; private set; }
    public DateOnly CreatedAt { get; init; }
    public string? Avatar { get; set; }
    public string? Country { get; set; }
    public Gender? Gender { get; set; }

    public int? RankId { get; private set; }

    public Rank? Rank { get; private set; }
    public UserStats Stats { get; private set; } = new();
    public ICollection<Trip> Trips { get; set; } = [];
    public ICollection<RegionProgress> RegionProgresses { get; set; } = [];

    public User UpdateRegionProgress(UpdateRegionProgress progressUpdate) {
        var regionToUpdate = RegionProgresses.FirstOrDefault(rp =>
            rp.RegionId == progressUpdate.RegionId
        );

        if (regionToUpdate is not null) {
            regionToUpdate.AddPeakVisits(progressUpdate.PeaksIds);
        }

        return this;
    }

    public User SetBirthday(DateOnly? date) {
        if (date is null) {
            return this;
        }

        if (date < DateOnly.FromDateTime(DateTime.Now)) {
            BirthDay = date.Value;
        }

        return this;
    }

    public static User DemoUser =>
        new() {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            UserName = "defaultuser",
            FirstName = "Default",
            LastName = "User",
            Email = "default@gmail.com",
            EmailConfirmed = true,
        };
}
