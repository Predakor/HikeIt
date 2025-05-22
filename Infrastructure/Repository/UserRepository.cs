using Domain.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class UserRepository : Repository<User>, IUserRepository {
    public UserRepository(TripDbContext context)
        : base(context) { }

    public async Task<List<User>> GetAllUsers() {
        return await DbSet.ToListAsync();
    }

    public async Task<User?> GetUser(int id) {
        return await DbSet.FindAsync(id);
    }
    public async Task<bool> Create(User newUser) {
        try {
            await DbSet.AddAsync(newUser);
            return true;
        }
        catch (Exception) {
            return false;
        }

    }
}
