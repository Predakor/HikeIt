using Domain.Common;
using Domain.Common.Result;
using Domain.Users;
using Domain.Users.ValueObjects;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Aggregates.Users;

public class UserRepository : Repository<User, Guid>, IUserRepository {
    public UserRepository(TripDbContext context)
        : base(context) { }

    public async Task<bool> Create(User newUser) {
        // Add Validation
        await DbSet.AddAsync(newUser);
        return await SaveChangesAsync();
    }

    public async Task<Result<bool>> UpdateStats(Guid userId, StatsUpdates.All update) {
        var user = await DbSet.FindAsync(userId);
        if (user is null) {
            return Errors.NotFound($"User with id: {userId}");
        }

        user.Stats.AddStats(update);

        var succes = await SaveChangesAsync();

        if (!succes) {
            return Errors.DbError("Failed to save stats update");
        }

        return await SaveChangesAsync();
    }
}
