using Domain.TripAnalytics;
using Domain.Trips.ValueObjects;

namespace Application.Services;

internal interface ITripAnalyticService {
    Task<TripAnalytic> GenerateAnalytic(TripAnalyticData data);
}