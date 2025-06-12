using Domain.ReachedPeaks;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Repositories;
using Infrastructure.Data;

namespace Infrastructure.UnitOfWorks;

public class TripAnalyticsUnitOfWork : ITripAnalyticUnitOfWork {
    readonly TripDbContext _dbContext;

    public ITripAnalyticRepository TripAnalytics { get; init; }
    public IElevationProfileRepository Elevations { get; init; }
    public IReachedPeakRepository PeaksAnalytics { get; init; }

    public TripAnalyticsUnitOfWork(
        TripDbContext dbContext,
        ITripAnalyticRepository tripAnalytics,
        IElevationProfileRepository elevations,
        IReachedPeakRepository peaksAnalytics
    ) {
        _dbContext = dbContext;
        TripAnalytics = tripAnalytics;
        Elevations = elevations;
        PeaksAnalytics = peaksAnalytics;
    }

    public Task<int> SaveChangesAsync() {
        return _dbContext.SaveChangesAsync();
    }
}
