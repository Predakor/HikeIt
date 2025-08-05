using Application.Trips;
using Domain.Common.Result;
using Domain.Trips;

namespace Application.Services.Trips;

public interface ITripService {
    public Task<Result<Trip>> CreateAsync(Trip trip);
    public Task<Result<Trip>> CreateAsync(CreateTripContext ctx);
    public Task<Result<Guid>> CreateSimpleAsync(CreateTripContext ctx);
    public Task<Result<bool>> DeleteAsync(Guid id, Guid userId);
}
