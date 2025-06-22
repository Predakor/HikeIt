using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Common.Result;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics.Commands;

public class CreateElevationProfileCommand : ICommand<ElevationProfile> {
    readonly AnalyticData _data;
    readonly Guid _id;

    CreateElevationProfileCommand(AnalyticData data, Guid id) {
        _data = data;
        _id = id;
    }

    public Result<ElevationProfile> Execute() {
        var points = _data.Points;

        if (points.Count == 0) {
            var error = Errors.EmptyCollection("elevation data");
            return error;
        }

        var profile = ElevationProfile.Create(_id, points.First(), points.ToGains());
        return profile;
    }

    public static ICommand<ElevationProfile> Create(AnalyticData data, Guid id) {
        return new CreateElevationProfileCommand(data, id);
    }
}
