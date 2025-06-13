using Domain.TripAnalytics;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics.Interfaces;

public interface ITripAnalyticService {
    Task<TripAnalytic> GenerateAnalytic(AnalyticData data);
    Task<TripAnalytic?> GetAnalytic(Guid id);
}