using Application.Trips.Analytics.Pipeline.Contracts;
using Application.Trips.Analytics.Pipeline.Runners;
using Application.Trips.Root.ValueObjects;
using Domain.Common.Geography.ValueObjects;
using Domain.Trips.Analytics.ElevationProfiles;
using Domain.Trips.Analytics.ElevationProfiles.Commands;

namespace Application.Trips.Analytics.ElevationProfiles;

public class ElevationProfileService : IElevationProfileService
{
    private readonly IPointsFilterRunner _pointsRunner;

    public ElevationProfileService(IPointsFilterRunner pointsRunner)
    {
        _pointsRunner = pointsRunner;
    }

    public Task<Result<ElevationProfile>> Create(CreateTripContext ctx)
    {
        return _pointsRunner
            .ApplyAsync(ctx.AnalyticData.Points, StageId.ElevationProfile)
            .BindAsync(points => CreateElevationProfileCommand.Create(new AnalyticData(points), ctx.Trip.Id).Execute());
    }
}