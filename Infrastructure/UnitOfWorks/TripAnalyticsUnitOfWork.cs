using Domain.Common;
using Domain.Common.Result;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Interfaces;
using Domain.TripAnalytics.Repositories;
using Domain.Trips;
using Domain.Trips.Entities.GpxFiles;
using Domain.Users;
using Infrastructure.Aggregates.Users;
using Infrastructure.Data;
using Infrastructure.Repository;

namespace Infrastructure.UnitOfWorks;

public class TripAnalyticsUnitOfWork : ITripAnalyticUnitOfWork {
    readonly TripDbContext _dbContext;

    public IPeakAnalyticRepository PeakAnalytics { get; }
    public IElevationProfileRepository Elevations { get; }
    public ITripAnalyticRepository TripAnalytics { get; }
    public IReachedPeakRepository ReachedPeaks { get; }
    public ITripRepository TripRepository { get; }
    public IUserRepository UserRepository { get; }
    public IGpxFileRepository GpxFileRepository { get; }

    public TripAnalyticsUnitOfWork(TripDbContext dbContext) {
        _dbContext = dbContext;
        Elevations = new ElevationProfileRepository(_dbContext);
        TripAnalytics = new TripAnalyticRepository(_dbContext);
        ReachedPeaks = new ReachedPeakRepository(_dbContext);
        PeakAnalytics = new PeakAnalyticRepository(_dbContext);
        TripRepository = new TripRepository(_dbContext);
        UserRepository = new UserRepository(_dbContext);
        GpxFileRepository = new GpxFileRepository(_dbContext);
    }

    public async Task<Result<bool>> SaveChangesAsync() {
        var isSaved = await _dbContext.SaveChangesAsync() > 0;

        return isSaved
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Errors.DbError("Internal error"));
    }
}
