using Application.Trips.Root.Dtos;
using Application.Trips.Root.ValueObjects;
using Domain.Trips.Root;

namespace Application.Trips.Root.Services;

public interface ITripService {
    public Task<Result<Trip>> CreateAsync(Trip trip);
    public Task<Result<Trip>> CreateAsync(CreateTripContext ctx);
    public Task<Result<Guid>> CreateSimpleAsync(CreateTripContext ctx);
    public Task<Result<bool>> DeleteAsync(Guid id, Guid userId);
    public Task<Result<bool>> UpdateAsync(Guid id, Guid userId, UpdateTripDto update);

}
