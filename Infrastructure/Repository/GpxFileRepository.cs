using Domain.GpxFiles;
using Infrastructure.Data;

namespace Infrastructure.Repository;
public class GpxFileRepository : Repository<GpxFile>, IGpxFileRepository {
    public GpxFileRepository(TripDbContext context) : base(context) {
    }

    public async Task<bool> AddAsync(GpxFile entity) {
        await DbSet.AddAsync(entity);
        return await SaveChangesAsync();
    }

    public async Task<bool> RemoveAsync(int id) {
        var target = await DbSet.FindAsync(id);
        if (target == null) {
            return false;
        }
        DbSet.Remove(target);
        return await SaveChangesAsync();
    }

    public Task<bool> UpdateAsync(int id, GpxFile updatedEntity) {
        throw new NotImplementedException();
    }
}
