using HikeIt.Api.Entities;
using HikeIt.Api.Repository;

namespace HikeIt.Api.Endpoints;

public static class RegionsEndpoints {
    public static RouteGroupBuilder MapRegionsEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/regions");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapPost("/", CreateRegion).Produces<Region>();

        return group;
    }

    static async Task<IResult> GetAll(IRepository<Region> repo) {
        var results = await repo.GetAllAsync();
        if (results is null) {
            return Results.NotFound();
        }
        return Results.Ok(results);
    }
    static async Task<IResult> GetById(IRepository<Region> repo, int id) {
        var result = await repo.GetByIDAsync(id);
        if (result is null) {
            return Results.NotFound();
        }
        return Results.Ok(result);
    }

    static async Task<IResult> CreateRegion(IRepository<Region> repo, Region region) {
        await repo.AddAsync(region);
        return Results.Ok();
    }

}

public static class ResoureManager {

}

