using Domain.TripAnalytics;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics;

public interface ITripAnalyticService {
    Task<TripAnalytic> GenerateAnalytic(TripAnalyticData data);
    Task<TripAnalytic?> GetAnalytic(Guid id);
}