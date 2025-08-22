using Application.Trips.Root.ValueObjects;
using Domain.Common.Result;
using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.ElevationProfiles.Commands;
using Domain.Trips.Root.Builders.Config;

namespace Application.Trips.Analytics.ElevationProfiles;

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
