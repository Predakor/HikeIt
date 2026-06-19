using Application.Locations.Regions;
using Application.Users.RegionProgressions.Dtos;
using Application.Users.Root.Dtos;
using Application.Users.Stats;
using Application.Users.Stats.Dtos;
using Domain.Users.RegionProgressions;
using Domain.Users.Root;
using Infrastructure.Commons.Databases;
using Infrastructure.Commons.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users.Stats;

internal sealed class UserQueryService(TripDbContext _dbContext) : IUserQueryService
{
    private IQueryable<User> Users => _dbContext.Users.AsNoTracking();
    private IQueryable<RegionProgress> RegionProgresses => _dbContext.Set<RegionProgress>().AsNoTracking();

    public Task<Result<UserStatsDto.All>> GetStats(Guid userId)
    {
        return Users
            .Where(u => u.Id == userId)
            .Select(u => new UserStatsDto.All(
                new UserStatsDto.Totals(
                    TotalDistanceMeters: u.Stats.TotalDistanceMeters,
                    TotalAscentMeters: u.Stats.TotalAscentMeters,
                    TotalDescentMeters: u.Stats.TotalDescentMeters,
                    TotalDuration: u.Stats.TotalDuration,
                    TotalClimbDuration: u.Stats.TotalClimbDuration,
                    TotalDescentDuration: u.Stats.TotalDescentDuration,
                    TotalPeaks: u.Stats.TotalPeaks,
                    TotalTrips: u.Stats.TotalTrips
                ),
                new UserStatsDto.Locations(u.Stats.UniquePeaks, u.Stats.RegionsVisited),
                new UserStatsDto.Metas(
                    u.Stats.FirstHikeDate,
                    u.Stats.LastHikeDate,
                    u.Stats.LongestTripMeters,
                    u.Stats.LongestTripMinutes
                )
            ))
            .FirstOrFailureAsync(userId, nameof(User), CancellationToken.None);

    }

    public Task<Result<UserDto.Profile>> GetProfile(Guid userId, CancellationToken ct)
    {
        return Users
            .Where(u => u.Id == userId)
            .Select(u => new UserDto.Profile(
                new UserDataDto.PublicProfile
                {
                    UserName = u.UserName!,
                    Avatar = u.Avatar,
                    Rank = u.Rank != null ? u.Rank.Name : "Novice Hiker",
                    Peaks = u.Stats.TotalPeaks,
                    Trips = u.Stats.TotalTrips,
                    Traveled = u.Stats.TotalDistanceMeters,
                },
                new UserDataDto.Personal
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email ?? "",
                    BirthDay = u.BirthDay,
                    Country = u.Country ?? "",
                    Gender = u.Gender.ToString(),
                },
                new UserDataDto.AccountState
                {
                    Role = "user",
                    CreatedAt = u.CreatedAt,
                    Status = "Active",
                }
            ))
            .FirstOrFailureAsync(userId, nameof(User), "id", ct);
    }

    public Task<Result<RegionProgressDto.Summary[]>> GetRegionsSummaries(Guid userId)
    {
        return RegionProgresses
            .Where(rp => rp.UserId == userId)
            .Select(rp => new RegionProgressDto.Summary(
                new RegionDto.Complete(rp.Region.Id, rp.Region.Name),
                rp.UniqueReachedPeaks,
                rp.TotalPeaksInRegion
            ))
            .ToResultArrayAsync("region progresses", CancellationToken.None);

    }
}

