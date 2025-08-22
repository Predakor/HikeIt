using Application.Trips.Analytics.Dtos;
using Application.Trips.Root.Dtos;
using Application.Trips.Root.Queries;
using Domain.Common;
using Domain.Common.Result;
using Domain.Trips.Root;
using Infrastructure.Commons.Databases;
using Infrastructure.Trips.Root.Queries;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Trips.Root.Queries;

public class TripQueryService : ITripQueryService {
    readonly TripDbContext _context;
    IQueryable<Trip> Trips => _context.Trips.AsNoTracking();

    public TripQueryService(TripDbContext context) {
        _context = context;
    }

    public async Task<Result<TripDto.WithBasicAnalytics>> GetWithBasicAnalytics(
        Guid id,
        Guid userId
    ) {
        var trip = await Trips
            .Include(t => t.Analytics)
            .Where(t => t.Id == id && t.UserId == userId)
            .FirstOrDefaultAsync();

        if (trip is null) {
            return Errors.NotFound("trip", id);
        }

        return trip.ToWithBasicAnalyticsDto();
    }

    public async Task<Result<List<TripDto.Summary>>> GetAllAsync(Guid userId) {
        var trips = await Trips.Where(t => t.UserId == userId).Include(t => t.Region).ToListAsync();

        if (trips.Count == 0)
            return Errors.EmptyCollection("user has no trips");

        return trips.Select(q => q.ToSummaryDto()).ToList();
    }

    public async Task<Result<TripDto.Partial>> GetByIdAsync(Guid id, Guid userId) {
        var query = await Trips.Where(t => t.UserId == userId).FirstOrDefaultAsync(t => t.Id == id);

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

    public static TripDto.WithBasicAnalytics ToWithBasicAnalyticsDto(this Trip trip) {
        if (trip.Analytics is null) {
            throw new Exception("trip has no analytics");
        }

        return new(trip.Id, trip.Name, trip.TripDay, trip.Analytics.ToBasicDto());
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
