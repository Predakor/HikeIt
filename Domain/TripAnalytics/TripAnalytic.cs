using Domain.TripAnalytics.ValueObjects.PeaksAnalytics;
using Domain.TripAnalytics.ValueObjects.RouteAnalytics;
using Domain.TripAnalytics.ValueObjects.TimeAnalytics;
using Domain.Trips.Builders.GpxDataBuilder;
using Domain.Trips.Factories;
using Domain.Trips.ValueObjects;

namespace Domain.TripAnalytics;
public class TripAnalytic : IEntity<Guid> {
    public Guid Id { get; init; }

    #region Owned Types
    public RouteAnalytic? TripAnalytics { get; private set; }
    public TripTimeAnalytic? TimeAnalytics { get; private set; }
    public PeaksAnalytic? PeaksAnalytics { get; private set; }
    #endregion
    public static TripAnalytic Create(TripAnalyticData data) {
        var gpxData = GpxDataBuilder.ProcessData(data);
        var analytics = TripAnalyticFactory.CreateAnalytics(gpxData);

        return new TripAnalytic();
    }
}
