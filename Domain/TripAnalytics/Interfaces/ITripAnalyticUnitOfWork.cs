using Domain.Common;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Repositories;
using Domain.Trips;

namespace Domain.TripAnalytics.Interfaces;
public interface ITripAnalyticUnitOfWork {
    public ITripRepository TripRepository { get; }
    public ITripAnalyticRepository TripAnalytics { get; }
    public IElevationProfileRepository Elevations { get; }
    public IReachedPeakRepository ReachedPeaks { get; }

    Task<Result<bool>> SaveChangesAsync();
}

