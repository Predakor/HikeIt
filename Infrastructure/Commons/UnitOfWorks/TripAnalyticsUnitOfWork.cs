using Domain.Common;
using Domain.Common.Result;
using Domain.ReachedPeaks;
using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.Peaks;
using Domain.Trips.Analytics.Root.Interfaces;
using Domain.Trips.Root;
using Domain.Users.Root;
using Infrastructure.Commons.Databases;
using Infrastructure.ReachedPeaks;
using Infrastructure.Trips.Analytics;
using Infrastructure.Trips.Analytics.ElevationProfiles;
using Infrastructure.Trips.Analytics.Peaks;
using Infrastructure.Trips.Root;
using Infrastructure.Users.Root;

namespace Infrastructure.Commons.UnitOfWorks;

public class TripAnalyticsUnitOfWork : ITripAnalyticUnitOfWork {
    readonly TripDbContext _dbContext;

    public IPeakAnalyticRepository PeakAnalytics { get; }
    public IElevationProfileRepository Elevations { get; }
    public ITripAnalyticRepository TripAnalytics { get; }
    public IReachedPeakRepository ReachedPeaks { get; }
    public ITripRepository TripRepository { get; }
    public IUserRepository UserRepository { get; }

    public TripAnalyticsUnitOfWork(TripDbContext dbContext) {
        _dbContext = dbContext;
        Elevations = new ElevationProfileRepository(_dbContext);
        TripAnalytics = new TripAnalyticRepository(_dbContext);
        ReachedPeaks = new ReachedPeakRepository(_dbContext);
        PeakAnalytics = new PeakAnalyticRepository(_dbContext);
        TripRepository = new TripRepository(_dbContext);
        UserRepository = new UserRepository(_dbContext);
    }

    public async Task<Result<bool>> SaveChangesAsync() {
        var isSaved = await _dbContext.SaveChangesAsync() > 0;

        return isSaved
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Errors.DbError("Internal error"));
    }
}
