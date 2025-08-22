using Domain.Users.RegionProgressions;
using Domain.Users.Root;
using Domain.Users.Stats;
using Domain.Users.Stats.Extentions;
using Domain.Users.Stats.ValueObjects;
using Infrastructure.Commons.Databases;
using Infrastructure.Commons.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Users.Root;

public enum UpdateType {
    Increase,
    Decrease,
}

public class UserRepository : CrudResultRepository<User, Guid>, IUserRepository {
    readonly DbSet<UserStats> _stats;
    readonly DbSet<RegionProgress> _regionsProgress;

    public UserRepository(TripDbContext context)
        : base(context) {
        _stats = context.Set<UserStats>();
        _regionsProgress = context.Set<RegionProgress>();
    }

    public async Task<bool> Create(User newUser) {
        await DbSet.AddAsync(newUser);
        return true;
    }

    public async Task<Result<User>> GetWithRegionProgresses(Guid userId) {
        var user = await DbSet
            .Include(u => u.RegionProgresses)
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();

        if (user is null) {
            return Errors.NotFound("user", userId);
        }

        return user;
    }

    public async Task<Result<bool>> UpdateStats(
        Guid userId,
        UserStatsUpdates.All update,
        UpdateMode updateMode = UpdateMode.Increase
    ) {
        return await GetUserStats(userId)
            .TapAsync(s => s.UpdateStats(update, updateMode))
            .BindAsync(_ => SaveChangesAsync());
    }

    public async Task<Result<RegionProgress>> CreateRegionProgress(RegionProgress regionProgres) {
        await _regionsProgress.AddAsync(regionProgres);
        return regionProgres;
    }

    public async Task<Result<UserStats>> GetUserStats(Guid userId) {
        var stats = await _stats.FindAsync(userId);
        if (stats is null) {
            Console.WriteLine("Passed not existing user");
            return Errors.NotFound("user", userId);
        }
        return stats;
    }
}
