using Domain.Common;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Repositories;
using Domain.Trips;
using Infrastructure.Data;

namespace Infrastructure.UnitOfWorks;

public class TripAnalyticsUnitOfWork : ITripAnalyticUnitOfWork {
    readonly TripDbContext _dbContext;

    public ITripAnalyticRepository TripAnalytics { get; init; }
    public IElevationProfileRepository Elevations { get; init; }
    public IReachedPeakRepository ReachedPeaks { get; init; }
    public ITripRepository TripRepository { get; init; }

    public TripAnalyticsUnitOfWork(
        TripDbContext dbContext,
        ITripAnalyticRepository tripAnalytics,
        IElevationProfileRepository elevations,
        IReachedPeakRepository peaksAnalytics,
        ITripRepository tripRepository
    ) {
        _dbContext = dbContext;
        TripAnalytics = tripAnalytics;
        Elevations = elevations;
        ReachedPeaks = peaksAnalytics;
        TripRepository = tripRepository;
    }

    public async Task<Result<bool>> SaveChangesAsync() {
        var isSaved = await _dbContext.SaveChangesAsync() > 0;

        return isSaved
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Errors.DbError("Internal error"));
    }
}
