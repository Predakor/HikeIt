using Domain.Common;
using Domain.Interfaces;

namespace Domain.TripAnalytics.Interfaces;
public interface ITripAnalyticRepository : ICrudRepository<TripAnalytic, Guid> {
    public Task<Result<TripAnalytic>> GetCompleteAnalytic(Guid id);
}
