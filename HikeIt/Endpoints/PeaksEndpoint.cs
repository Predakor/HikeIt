using HikeIt.Api.Entities;
using HikeIt.Api.Repository;

namespace HikeIt.Api.Endpoints;

public static class PeaksEndpoint {
    public static RouteGroupBuilder MapPeaksEndpoint(this WebApplication app) {
        var group = app.MapGroup("peaks");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);

        return group;
    }

    static async Task<IResult> GetAll(IRepository<Peak> repo) {
        var peaks = await repo.GetAllAsync();
        return Results.Ok(peaks);
    }

    static async Task<IResult> GetById(int id, IRepository<Peak> repo) {
        var peak = await repo.GetByIDAsync(id);
        return Results.Ok(peak);
    }
}
