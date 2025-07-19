using Application.Dto;
using Application.Services.Trips;
using Domain.Common;
using Domain.Common.Result;
using Domain.Trips;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.TripAnalytics.Queries;

public class TripQueryService : ITripQueryService {
    readonly TripDbContext _context;
    readonly DbSet<Trip> _trips;

    public TripQueryService(TripDbContext context) {
        _context = context;
        _trips = context.Trips;
    }

    public Task<Result<TripDto.WithBasicAnalytics>> GetWithBasicAnalytics(Guid id, Guid userId) {
        throw new NotImplementedException();
    }

    public async Task<Result<List<TripDto.Summary>>> GetAllAsync(Guid userId) {
        var trips = await _trips
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .Include(t => t.Region)
            .ToListAsync();

        if (trips.Count == 0)
            return Errors.NotFound("user has no trips");

        return trips.Select(q => q.ToSummaryDto()).ToList();
    }

    public async Task<Result<TripDto.Partial>> GetByIdAsync(Guid id, Guid userId) {
        var query = await _trips
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (query == null) {
            return Errors.NotFound("trip with id:" + id);
        }

        return query.ToPartialDto();
    }
}

static class Helpers {
    public static TripDto.Summary ToSummaryDto(this Trip trip) {
        return new(trip.Id, trip.Name, trip.TripDay, trip.Region);
    }

    public static TripDto.Request.ResponseBasic ToBasicDto(this Trip trip) {
        return new(trip.Id, trip.RegionId, new(trip.Name, trip.TripDay));
    }

    public static TripDto.Partial ToPartialDto(this Trip trip) {
        return new(
            trip.Id,
            trip.Analytics,
            trip.GpxFile,
            trip.Region,
            new(trip.Name, trip.TripDay)
        );
    }

    public static List<TripDto.Request.ResponseBasic> CollectionToDtos(IEnumerable<Trip> trips) {
        return [.. trips.Select(p => p.ToBasicDto())];
    }
}
