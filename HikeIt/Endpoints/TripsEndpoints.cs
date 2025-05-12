using HikeIt.Api.Entities;
using HikeIt.Api.Repository;

namespace HikeIt.Api.Endpoints;

public static class TripsEndpoints
{
    //extension Method

    public static RouteGroupBuilder MapTripsEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("trips").WithParameterValidation();

        group.MapGet("/", GetAll);

        group.MapGet("/{id}", GetByID);

        group.MapPost("/", CreateTrip);

        group.MapPut("/{id}", UpdateTrip);

        group.MapDelete("/{id}", DeleteTrip);

        return group;
    }

    static async Task<IResult> GetAll(IRepository<Trip> repo)
    {
        var trips = await repo.GetAllAsync();
        return Results.Ok(trips);
    }

    static async Task<IResult> GetByID(IRepository<Trip> repo, Guid id)
    {
        var trip = await repo.GetByIDAsync(id);
        if (trip is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(trip);
    }

    static async Task<IResult> CreateTrip(Trip newTrip, IRepository<Trip> repo)
    {
        await repo.AddAsync(newTrip);
        return Results.Created($"/trips/{newTrip.Id}", newTrip);
    }

    static async Task<IResult> UpdateTrip(Guid id, Trip updatedTrip, IRepository<Trip> repo)
    {
        await repo.UpdateAsync(id, updatedTrip);
        return Results.NoContent();
    }

    static async Task<IResult> DeleteTrip(Guid id, IRepository<Trip> repo)
    {
        await repo.RemoveAsync(id);
        return Results.NoContent();
    }
}
