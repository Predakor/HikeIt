using Application.Trips.Root.ValueObjects;
using Domain.Trips.Analytics.Root;

namespace Application.Trips.Analytics.Interfaces;

public interface ITripAnalyticService {
    Task<Result<TripAnalytic>> GenerateAnalytic(CreateTripContext ctx);
}
