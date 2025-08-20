using Domain.Common.Result;
using Domain.Interfaces;

namespace Domain.Trips;

public interface ITripRepository : IRepository<Trip, Guid> {
    Task<Result<IEnumerable<Trip>>> GetAll(Guid userId);
    Task<Result<Trip>> GetByIdAsync(Guid tripId);
    Task<Result<Trip>> GetWithFile(Guid tripId);
    Task<Result<Trip>> Get(Guid tripId, Guid userId);
    Result<Trip> Add(Trip trip);
    Result<bool> Remove(Trip trip);
}
