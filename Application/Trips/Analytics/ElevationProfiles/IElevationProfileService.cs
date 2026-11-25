using Application.Trips.Root.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles;

namespace Application.Trips.Analytics.ElevationProfiles;

public interface IElevationProfileService : IService
{
    Task<Result<ElevationProfile>> Create(CreateTripContext ctx);
}
