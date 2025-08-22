using Domain.Common.Abstractions;
using Domain.Common.Result;

namespace Domain.Trips.Root;

public interface ITripRepository : IRepository<Trip, Guid> {
    Task<Result<IEnumerable<Trip>>> GetAll(Guid userId);
    Task<Result<Trip>> GetByIdAsync(Guid tripId);
    Task<Result<Trip>> GetWithFile(Guid tripId);
    Task<Result<Trip>> Get(Guid tripId, Guid userId);
    Result<Trip> Add(Trip trip);
    Result<bool> Remove(Trip trip);
    Task<Result<bool>> SaveChangesAsync();
}
