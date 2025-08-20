using Domain.FileReferences;
using Domain.Trips.Entities.GpxFiles;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository;

public class GpxFileRepository : Repository<FileReference, Guid>, IGpxFileRepository {
    public GpxFileRepository(TripDbContext context)
        : base(context) { }

    public async Task<FileReference?> GetGpxFile(Guid id) {
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

        return File.OpenRead(result.Url);
    }

    public async Task<bool> AddAsync(FileReference entity) {
        await DbSet.AddAsync(entity);
        return true;
    }
    public FileReference Add(FileReference entity) {
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
