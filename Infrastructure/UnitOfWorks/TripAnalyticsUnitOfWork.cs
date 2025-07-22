using Domain.Common;
using Domain.Common.Result;
using Domain.Entiites.Users;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Repositories;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using Infrastructure.Data;

namespace Infrastructure.UnitOfWorks;

public class TripAnalyticsUnitOfWork : ITripAnalyticUnitOfWork {
    readonly TripDbContext _dbContext;

    public IPeakAnalyticRepository PeakAnalytics { get; init; }
    public IElevationProfileRepository Elevations { get; init; }
    public ITripAnalyticRepository TripAnalytics { get; init; }
    public IReachedPeakRepository ReachedPeaks { get; init; }
    public ITripRepository TripRepository { get; init; }
    public IUserRepository UserRepository { get; init; }
    public IGpxFileRepository GpxFileRepository { get; init; }

    public TripAnalyticsUnitOfWork(
        TripDbContext dbContext,
        ITripRepository tripRepository,
        IUserRepository userRepository,
        IPeakAnalyticRepository peakAnalytics,
        IReachedPeakRepository peaksAnalytics,
        ITripAnalyticRepository tripAnalytics,
        IElevationProfileRepository elevations,
        IGpxFileRepository gpxFileRepository
    ) {
        _dbContext = dbContext;
        Elevations = elevations;
        TripAnalytics = tripAnalytics;
        ReachedPeaks = peaksAnalytics;
        PeakAnalytics = peakAnalytics;
        TripRepository = tripRepository;
        UserRepository = userRepository;
        GpxFileRepository = gpxFileRepository;
    }

    public async Task<Result<bool>> SaveChangesAsync() {
        var isSaved = await _dbContext.SaveChangesAsync() > 0;

        return isSaved
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Errors.DbError("Internal error"));
    }
}
