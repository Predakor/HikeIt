using Application.Commons.Drafts;
using Domain.TripAnalytics;
using Domain.Trips;

namespace Application.Trips;

public class TripDraft : IDraft {
    public Guid Id => Trip.Id;
    public Guid UserId => Trip.UserId;

    public Trip Trip { get; private set; }

    public TripDraft AddAnalytics(TripAnalytic analytics) {
        if (analytics is not null) {
            Trip.AddAnalytics(analytics);
        }

        return this;
    }

    public static TripDraft Create(Guid userId) {
        Guid tripId = Guid.NewGuid();

        return new TripDraft { Trip = Trip.Create(tripId, "", DateOnly.MinValue, userId) };
    }
}
