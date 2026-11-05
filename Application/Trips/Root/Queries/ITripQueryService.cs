using Application.Commons.Abstractions.Queries;
using Application.Trips.Root.Dtos;

namespace Application.Trips.Root.Queries;

public interface ITripQueryService : IQueryService
{
    Task<Result<TripDto.Partial>> GetByIdAsync(Guid id, Guid userId);
    Task<Result<TripDto.WithBasicAnalytics>> GetWithBasicAnalytics(Guid id, Guid userId);
    Task<Result<List<TripDto.Summary>>> GetSummariesAsync(Guid userId);

    // Admin-only: searches across every user's trips by name, not just the current user's own.
    Task<Result<List<TripDto.AdminSearchResult>>> SearchForAdminAsync(string search);
}
