using Application.Trips.Root.Dtos;
using Application.Trips.Root.ValueObjects;
using Domain.Trips.Root;

namespace Application.Trips.Root.Services;

public interface ITripService
{
    Task<Result<Trip>> CreateAsync(Trip trip);
    Task<Result<Trip>> CreateAsync(CreateTripContext ctx);
    Task<Result<Guid>> CreateSimpleAsync(CreateTripContext ctx);
    Task<Result<bool>> DeleteAsync(Guid id, Guid userId);
    Task<Result<bool>> UpdateAsync(Guid id, Guid userId, UpdateTripDto update);
}
