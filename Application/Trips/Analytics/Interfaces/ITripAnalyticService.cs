using Application.Trips.Root.ValueObjects;
using Domain.Common.Result;
using Domain.Trips.Analytics.Root;

namespace Application.Trips.Analytics.Interfaces;

public interface ITripAnalyticService {
    Task<Result<TripAnalytic>> GenerateAnalytic(CreateTripContext ctx);
}
