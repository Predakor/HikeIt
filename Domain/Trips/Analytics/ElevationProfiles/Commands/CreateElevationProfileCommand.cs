using Domain.Common.Abstractions;
using Domain.Common.Geography.Extentions;
using Domain.Common.Geography.ValueObjects;

namespace Domain.Trips.Analytics.ElevationProfiles.Commands;

public class CreateElevationProfileCommand : ICommand<ElevationProfile>
{
    private readonly AnalyticData _data;
    private readonly Guid _id;

    private CreateElevationProfileCommand(AnalyticData data, Guid id)
    {
        _data = data;
        _id = id;
    }

    public static ICommand<ElevationProfile> Create(AnalyticData data, Guid id)
    {
        return new CreateElevationProfileCommand(data, id);
    }

    public Result<ElevationProfile> Execute()
    {
        var points = _data.Points;

        if (points.Count == 0)
        {
            var error = Errors.EmptyCollection("elevation data");
            return error;
        }

        var profile = ElevationProfile.Create(_id, points.First(), points.ToGains());
        return profile;
    }
}
