using Domain.Trips.Entities.GpxFiles;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class GpxFileRepository : Repository<GpxFile, Guid>, IGpxFileRepository {
    public GpxFileRepository(TripDbContext context) : base(context) { }

    public async Task<bool> AddAsync(GpxFile entity) {
        await DbSet.AddAsync(entity);
        return await SaveChangesAsync();
    }

    public async Task<bool> RemoveAsync(Guid id) {
        var target = await DbSet.FindAsync(id);
        if (target == null) {
            return false;
        }
        DbSet.Remove(target);
        return await SaveChangesAsync();
    }

    public Task<bool> UpdateAsync(Guid id, GpxFile updatedEntity) {
        throw new NotImplementedException();
    }

    public async Task<GpxFile?> GetGpxFile(Guid id) {
        var result = await DbSet.FindAsync(id);
        if (result != null) {
            return result;
        }

        return null;
    }

    public async Task<Stream?> GetGpxFileStream(Guid id) {
        var result = await DbSet.FindAsync(id);
        if (result == null) {
            return null;
        }

        return File.OpenRead(result.Path);
    }

}
