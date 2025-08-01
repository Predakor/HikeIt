using Application.Trips;
using Domain.Common.Result;
using Domain.TripAnalytics.Entities.ElevationProfile;

namespace Application.TripAnalytics.ElevationProfiles;

public interface IElevationProfileService {
    Result<ElevationProfile> Create(CreateTripContext ctx);
}
