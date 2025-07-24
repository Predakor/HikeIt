using Domain.Common;
using Domain.Common.Result;
using Domain.Users;
using Domain.Users.Entities;
using Domain.Users.Extentions;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Aggregates.Users;

public enum UpdateType {
    Increase,
    Decrease,
}

public class UserRepository : Repository<User, Guid>, IUserRepository {
    readonly DbSet<UserStats> _stats;

    public UserRepository(TripDbContext context)
        : base(context) {
        _stats = context.Set<UserStats>();
    }

    public async Task<bool> Create(User newUser) {
        // Add Validation
        await DbSet.AddAsync(newUser);
        return await SaveChangesAsync();
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

        var succes = await SaveChangesAsync();

        if (!succes) {
            Console.WriteLine("Db save failed or no entities were changed");
            return Errors.DbError("Failed to save stats update");
        }

        return succes;
    }
}
