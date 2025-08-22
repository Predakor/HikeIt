using Domain.Locations.Peaks;
using Infrastructure.Commons.Databases;
using Infrastructure.Commons.Repositories;

namespace Infrastructure.Locations.Peaks;
internal class PeaksRepository : CrudResultRepository<Peak, int>, IPeaksRepository {
    public PeaksRepository(TripDbContext context) : base(context) {

    }

}
