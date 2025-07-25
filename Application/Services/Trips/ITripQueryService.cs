using Application.Dto;
using Application.Interfaces;
using Domain.Common.Result;

namespace Application.Services.Trips;

public interface ITripQueryService : IQueryService {
    public Task<Result<TripDto.Partial>> GetByIdAsync(Guid id, Guid userId);
    public Task<Result<TripDto.WithBasicAnalytics>> GetWithBasicAnalytics(Guid id, Guid userId);
    public Task<Result<List<TripDto.Summary>>> GetAllAsync(Guid userId);
}
