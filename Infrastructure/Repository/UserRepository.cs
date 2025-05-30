using Domain.Users;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class UserRepository : Repository<User>, IUserRepository {
    public UserRepository(TripDbContext context)
        : base(context) { }

    public async Task<bool> Create(User newUser) {
        // Add Validation
        await DbSet.AddAsync(newUser);
        return await SaveChangesAsync();
    }
}
