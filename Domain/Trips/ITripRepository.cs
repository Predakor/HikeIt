using Domain.Interfaces;

namespace Domain.Trips;

public interface ITripRepository : ICrudRepository<Trip, Guid> {
}