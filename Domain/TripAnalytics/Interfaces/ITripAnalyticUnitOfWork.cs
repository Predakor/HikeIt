using Application.TripAnalytics.Interfaces;
using Domain.Common.Result;
using Domain.Entiites.Users;
using Domain.ReachedPeaks;
using Domain.TripAnalytics.Repositories;
using Domain.Trips;

namespace Domain.TripAnalytics.Interfaces;
public interface ITripAnalyticUnitOfWork {
    public IUserRepository UserRepository { get; }
    public ITripRepository TripRepository { get; }
    public ITripAnalyticRepository TripAnalytics { get; }
    public IElevationProfileRepository Elevations { get; }
    public IReachedPeakRepository ReachedPeaks { get; }
    public IPeakAnalyticRepository PeakAnalytics { get; }

    Task<Result<bool>> SaveChangesAsync();
}

