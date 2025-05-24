using Application.Dto;
using Application.Services.Trip;

namespace Api.Endpoints;

public static class TripsEndpoints {
    public static RouteGroupBuilder MapTripsEndpoint(this WebApplication app) {
        var group = app.MapGroup("api/trips").WithParameterValidation();

        group.MapGet("/", GetAll);

        group.MapGet("/{id}", GetByID);

        group.MapPost("/", CreateTrip);

        group.MapPut("/", UpdateTrip);

        group.MapDelete("/{id}", DeleteTrip);

        return group;
    }

    static async Task<IResult> GetAll(TripService service) {
        var trips = await service.GetAll();
        return Results.Ok(trips);
    }

    static async Task<IResult> GetByID(TripService service, int id) {
        var trip = await service.GetById(id);
        if (trip is null) {
            return Results.NotFound();
        }

        return Results.Ok(trip);
    }

    static async Task<IResult> CreateTrip(TripDto.CompleteLinked newTrip, TripService service) {
        await service.Add(newTrip);
        return Results.Created();
    }

    static async Task<IResult> UpdateTrip(TripDto.UpdateDto updateDto, TripService service) {
        await service.Update(updateDto);
        return Results.NoContent();
    }

    static async Task<IResult> DeleteTrip(int id, TripService service) {
        await service.Delete(id);
        return Results.NoContent();
    }
}
