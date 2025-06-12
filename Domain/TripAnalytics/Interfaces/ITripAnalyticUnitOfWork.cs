using Domain.ReachedPeaks;
using Domain.TripAnalytics.Repositories;

namespace Domain.TripAnalytics.Interfaces;
public interface ITripAnalyticUnitOfWork {

    public ITripAnalyticRepository TripAnalytics { get; }
    public IElevationProfileRepository Elevations { get; }
    public IReachedPeakRepository PeaksAnalytics { get; }


    Task<int> SaveChangesAsync();
}

