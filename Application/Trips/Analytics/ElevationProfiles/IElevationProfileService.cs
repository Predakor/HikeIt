using Application.Trips.Root.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles;

namespace Application.Trips.Analytics.ElevationProfiles;

public interface IElevationProfileService {
    Result<ElevationProfile> Create(CreateTripContext ctx);
}
