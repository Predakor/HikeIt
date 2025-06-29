using Domain.Trips.Entities.GpxFiles;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository;

public class GpxFileRepository : Repository<GpxFile, Guid>, IGpxFileRepository {
    public GpxFileRepository(TripDbContext context)
        : base(context) { }

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

    public async Task<bool> AddAsync(GpxFile entity) {
        await DbSet.AddAsync(entity);
        return true;
    }
    public GpxFile Add(GpxFile entity) {
        DbSet.Add(entity);
        return entity;
    }

    public async Task<bool> RemoveAsync(Guid id) {
        var target = await DbSet.FindAsync(id);
        if (target == null) {
            return false;
        }
        DbSet.Remove(target);
        return await SaveChangesAsync();
    }


}
