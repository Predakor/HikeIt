using Domain.Trips;
using Infrastructure.Data;

namespace Infrastructure.Repository;

public class TripRepository : Repository<Trip>, ITripRepository {
    public TripRepository(TripDbContext context)
        : base(context) { }

    public Task Add(Trip trip) {
        throw new NotImplementedException();
    }

    public Task Delete(Trip trip) {
        throw new NotImplementedException();
    }

    public Task<Trip> GetAsync(int id) {
        throw new NotImplementedException();
    }

    public Task Update(Trip trip) {
        throw new NotImplementedException();
    }
}
