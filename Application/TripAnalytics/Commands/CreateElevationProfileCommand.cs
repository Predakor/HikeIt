using Application.Commons.Interfaces;
using Domain.Common;
using Domain.TripAnalytics.Entities.ElevationProfile;
using Domain.Trips.Builders.GpxDataBuilder;

namespace Application.TripAnalytics.Commands;

internal class CreateElevationProfileCommand : ICommand<ElevationProfile> {
    readonly ElevationProfileData _data;

    public CreateElevationProfileCommand(ElevationProfileData eleData) {
        _data = eleData;
    }

    public Result<ElevationProfile> Execute() {
        var elevationData = GpxDataFactory.Create(_data);
        if (elevationData == null) {
            var error = Error.Unknown("Something went wrong while generating elevation profile");
            return Result<ElevationProfile>.Failure(error);
        }
        if (elevationData.Points.Count == 0) {
            var error = Error.Unknown("Looks like you elevation data is empty");
            return Result<ElevationProfile>.Failure(error);
        }

        var points = elevationData.Points;

        var profile = ElevationProfile.Create(points.First(), points.ToGains());

        return Result<ElevationProfile>.Success(profile);
    }

    public static CreateElevationProfileCommand Create(ElevationProfileData eleData) {
        return new CreateElevationProfileCommand(eleData);
    }
}
