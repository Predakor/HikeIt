using Domain.Common.Result;
using Domain.Interfaces;

namespace Domain.TripAnalytics.Interfaces;
public interface ITripAnalyticRepository : ICrudRepository<TripAnalytic, Guid> {
    Task<Result<TripAnalytic>> GetCompleteAnalytic(Guid id);
}
