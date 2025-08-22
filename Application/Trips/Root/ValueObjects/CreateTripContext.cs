using Domain.Common.Geography.ValueObjects;
using Domain.FileReferences.ValueObjects;
using Domain.Trips.Root;
using Domain.Users.Root;
using static Application.Trips.Root.Dtos.TripDto;

namespace Application.Trips.Root.ValueObjects;

public class CreateTripContext(Guid id) {
    public Guid Id { get; } = id;
    public User User { get; private set; }
    public FileContent File { get; private set; }
    public Request.Create Request { get; private set; }

    public Trip Trip { get; private set; }

    public AnalyticData AnalyticData { get; private set; }

    public CreateTripContext WithUser(User user) {
        User = user;
        return this;
    }

    public CreateTripContext WithFile(FileContent file) {
        File = file;
        return this;
    }

    public CreateTripContext WithRequest(Request.Create request) {
        Request = request;
        return this;
    }

    public CreateTripContext WithTrip(Trip trip) {
        Trip = trip;
        return this;
    }

    public CreateTripContext WithAnalyticData(AnalyticData data) {
        AnalyticData = data;
        return this;
    }

    public static CreateTripContext Create(Guid? id = null) {
        return new(id ?? Guid.NewGuid());
    }
}
