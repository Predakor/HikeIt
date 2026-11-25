using Domain.ReachedPeaks;
using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.Peaks;
using Domain.Trips.Root;
using Domain.Users.Root;

namespace Domain.Trips.Analytics.Root.Interfaces;

public interface ITripAnalyticUnitOfWork
{
    IUserRepository UserRepository { get; }
    ITripRepository TripRepository { get; }
    ITripAnalyticRepository TripAnalytics { get; }
    IElevationProfileRepository Elevations { get; }
    IReachedPeakRepository ReachedPeaks { get; }
    IPeakAnalyticRepository PeakAnalytics { get; }

    Task<Result<bool>> SaveChangesAsync();
}
