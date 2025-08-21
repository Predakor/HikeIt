using Application.Trips;
using Domain.Common.Result;
using Domain.TripAnalytics.Commands;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.TripAnalytics.Repositories;
using Domain.Trips.Config;

namespace Application.TripAnalytics.ElevationProfiles;

public class ElevationProfileService : IElevationProfileService {
    readonly IElevationProfileRepository _repository;

    public ElevationProfileService(IElevationProfileRepository repository) {
        _repository = repository;
    }

    public Result<ElevationProfile> Create(CreateTripContext ctx) {
        return CreateElevationProfileDataCommand
            .Create(ctx.AnalyticData, GpxDataConfigs.ElevationProfile)
            .Execute()
            .Bind(eleData => CreateElevationProfileCommand.Create(eleData, ctx.Trip.Id).Execute());
    }

}
