using Domain.TripAnalytics;
using Domain.Trips.ValueObjects;

namespace Application.Services;

public interface ITripAnalyticService {
    Task<TripAnalytic> GenerateAnalytic(TripAnalyticData data);
}