using Application.Commons.Interfaces;
using Domain.Common;
using Domain.TripAnalytics.Builders.TripAnalyticBuilder;
using Domain.Trips.ValueObjects;

namespace Application.TripAnalytics.Commands;


record PointsToPreserve(List<GpxPoint> Maximas, List<GpxPoint> Minimas);
record ElevationProfileData(AnalyticData Data, PointsToPreserve? Points);

internal class CreateElevationProfileCommand : ICommand<ElevationProfile> {
    //elevation Profile 
    //probbably a starting point
    //and array of deltas?
    public Result<ElevationProfile> Execute() {
        throw new NotImplementedException();
    }
}

public class ElevationProfileBuilder() {

}
