using Application.Commons.Abstractions.Queries;
using Application.Trips.Root.Dtos;

namespace Application.Trips.Root.Queries;

public interface ITripQueryService : IQueryService {
    public Task<Result<TripDto.Partial>> GetByIdAsync(Guid id, Guid userId);
    public Task<Result<TripDto.WithBasicAnalytics>> GetWithBasicAnalytics(Guid id, Guid userId);
    public Task<Result<List<TripDto.Summary>>> GetAllAsync(Guid userId);
}
