using Application.Trips;
using Domain.Common.Result;
using Domain.TripAnalytics;
using Domain.TripAnalytics.Entities.ElevationProfile;

namespace Application.TripAnalytics.Interfaces;

public interface ITripAnalyticService {
    Task<Result<TripAnalytic>> GenerateAnalytic(CreateTripContext ctx);
    Task<ElevationProfile> GetElevationProfile(Guid id);
}
