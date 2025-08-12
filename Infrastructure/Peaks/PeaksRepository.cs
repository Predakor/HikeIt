using Domain.Peaks;
using Infrastructure.Data;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Peaks;
internal class PeaksRepository : CrudResultRepository<Peak, int>, IPeaksRepository {
    public PeaksRepository(TripDbContext context) : base(context) {

    }

}
