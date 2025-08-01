using Domain.Common;
using Domain.Common.Result;
using Domain.Users;
using Domain.Users.Entities;
using Domain.Users.Extentions;
using Domain.Users.RegionProgresses;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Aggregates.Users;

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
        // Add Validation
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
        StatsUpdates.All update,
        UpdateMode updateMode = UpdateMode.Increase
    ) {
        var stats = await _stats.FindAsync(userId);
        if (stats is null) {
            Console.WriteLine("Passed not existing user");
            return Errors.NotFound($"User with id: {userId}");
        }

        stats.UpdateStats(update, updateMode);

        return await SaveChangesAsync();
    }

    public async Task<Result<RegionProgress>> CreateRegionProgress(RegionProgress regionProgres) {
        await _regionsProgress.AddAsync(regionProgres);
        return regionProgres;
    }
}
