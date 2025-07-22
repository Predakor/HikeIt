using Application.Trips;
using Domain.Common.Result;
using Domain.TripAnalytics;

namespace Application.TripAnalytics.Interfaces;

public interface ITripAnalyticService {
    Task<Result<TripAnalytic>> GenerateAnalytic(CreateTripContext ctx);
}
