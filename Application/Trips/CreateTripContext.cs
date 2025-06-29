using Domain.Entiites.Users;
using Domain.Trips;
using Domain.Trips.ValueObjects;
using Microsoft.AspNetCore.Http;
using static Application.Dto.TripDto;

namespace Application.Trips;

public class CreateTripContext(Guid id) {
    public Guid Id { get; } = id;
    public User User { get; private set; }
    public IFormFile File { get; private set; }
    public Request.Create Request { get; private set; }

    public Trip Trip { get; private set; }

    public AnalyticData AnalyticData { get; private set; }

    public CreateTripContext WithUser(User user) {
        User = user;
        return this;
    }

    public CreateTripContext WithFile(IFormFile file) {
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
