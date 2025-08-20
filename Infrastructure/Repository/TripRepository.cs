using Domain.Common;
using Domain.Common.Result;
using Domain.Trips;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class TripRepository : CrudResultRepository<Trip, Guid>, ITripRepository {
    public TripRepository(TripDbContext context)
        : base(context) { }

    public Result<Trip> Add(Trip trip) {
        DbSet.Add(trip);
        return trip;
    }

    public async Task<Result<Trip>> Get(Guid tripId, Guid userId) {
        var trip = await DbSet
            .Include(x => x.Region)
            .Include(x => x.GpxFile)
            .Include(x => x.Analytics)
            .ThenInclude(a => a.PeaksAnalytic)
            .Where(x => x.UserId == userId)
            .FirstOrDefaultAsync(x => x.Id == tripId);

        if (trip == null) {
            return Errors.NotFound($"trip with id:{tripId}");
        }

        return trip;
    }

    public async Task<Result<IEnumerable<Trip>>> GetAll(Guid userId) {
        return await DbSet
            .Include(x => x.Region)
            .Include(x => x.Analytics)
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<Result<Trip>> GetWithFile(Guid tripId) {
        var trip = await DbSet
            .Include(t => t.GpxFile)
            .Where(t => t.Id == tripId)
            .FirstOrDefaultAsync();

        if (trip is null) {
            return Errors.NotFound("trip", tripId);
        }

        return trip;
    }

    public Result<bool> Remove(Trip trip) {
        DbSet.Remove(trip);
        return true;
    }
}
