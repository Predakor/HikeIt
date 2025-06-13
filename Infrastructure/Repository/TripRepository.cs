using Application.Dto;
using Domain.Trips;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TripRepository : Repository<Trip, Guid>, ITripRepository {
    public TripRepository(TripDbContext context)
        : base(context) { }

    public override async Task<Trip?> GetByIdAsync(Guid id) {
        return await DbSet
            .Include(x => x.Region)
            .Include(x => x.GpxFile)
            .Include(x => x.Analytics)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddAsync(Trip entity) {
        return await DbSet.AddAsync(entity) != null;
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

        return await SaveChangesAsync();
    }
}
