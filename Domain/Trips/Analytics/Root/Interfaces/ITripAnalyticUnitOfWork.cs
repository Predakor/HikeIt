using Domain.ReachedPeaks;
using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.Peaks;
using Domain.Trips.Root;
using Domain.Users.Root;

namespace Domain.Trips.Analytics.Root.Interfaces;

public interface ITripAnalyticUnitOfWork {
    public IUserRepository UserRepository { get; }
    public ITripRepository TripRepository { get; }
    public ITripAnalyticRepository TripAnalytics { get; }
    public IElevationProfileRepository Elevations { get; }
    public IReachedPeakRepository ReachedPeaks { get; }
    public IPeakAnalyticRepository PeakAnalytics { get; }

    Task<Result<bool>> SaveChangesAsync();
}
