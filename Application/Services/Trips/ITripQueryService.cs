using Application.Dto;
using Domain.Common.Result;

namespace Application.Services.Trips;

public interface ITripQueryService {
    public Task<Result<TripDto.Partial>> GetByIdAsync(Guid id, Guid userId);
    public Task<Result<TripDto.WithBasicAnalytics>> GetWithBasicAnalytics(Guid id, Guid userId);
    public Task<Result<List<TripDto.Summary>>> GetAllAsync(Guid userId);
}
