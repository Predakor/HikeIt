using Application.Dto;
using Domain.Trips;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TripRepository : Repository<Trip, Guid>, ITripRepository {
    public TripRepository(TripDbContext context)
        : base(context) { }

    public override async Task<Trip?> GetByIdAsync(Guid id) {
        return await DbSet
            .Include(x => x.Region)
            .Include(x => x.GpxFile)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddAsync(Trip entity) {
        await DbSet.AddAsync(entity);
        await SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveAsync(Guid id) {
        var target = await DbSet.FindAsync(id);
        if (target == null) {
            return false;
        }

        DbSet.Remove(target);
        return await SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Guid id, TripDto.PartialLinked updateDto) {
        var target = await DbSet.FindAsync(id);
        if (target == null) {
            return false;
        }

        //TODO change it{
        if (updateDto.RegionId.HasValue) {
            target.ChangeRegion(updateDto.RegionId.Value);
        }

        if (updateDto.Base != null) {
            if (updateDto.Base.TripDay != null) {
                target.TripDay = updateDto.Base.TripDay.Value;
            }
            if (updateDto.Base.Distance != null) {
                target.Distance = updateDto.Base.Distance.Value;
            }
            if (updateDto.Base.Duration != null) {
                target.Duration = updateDto.Base.Duration.Value;
            }
        }
        if (updateDto.GpxFileId != null) {
            target.AddGpxFile(updateDto.GpxFileId.Value);
        }

        return await SaveChangesAsync();
    }

    public Task<bool> UpdateAsync(Guid id, Trip updatedEntity) {
        throw new NotImplementedException();
    }
}
