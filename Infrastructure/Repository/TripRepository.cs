using Domain.Trips;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TripRepository : Repository<Trip>, ITripRepository {
    public TripRepository(TripDbContext context)
        : base(context) { }

    public override async Task<Trip?> GetByIdAsync(int id) {
        return await DbSet.Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddAsync(Trip entity) {
        await DbSet.AddAsync(entity);
        await SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveAsync(int id) {
        var target = await DbSet.FindAsync(id);
        if (target == null) {
            return false;
        }

        DbSet.Remove(target);
        return await SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(int id, Trip updatedEntity) {
        var target = await DbSet.FindAsync(id);
        if (target == null) {
            return false;
        }

        target.Region = updatedEntity.Region;
        target.TripDay = updatedEntity.TripDay;
        target.Duration = updatedEntity.Duration;

        return await SaveChangesAsync();
    }
}
