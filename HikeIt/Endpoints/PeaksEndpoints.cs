using HikeIt.Api.Dto;
using HikeIt.Api.Entities;
using HikeIt.Api.Mappers.Implementations;
using HikeIt.Api.Repository;

namespace HikeIt.Api.Endpoints;

public static class PeaksEndpoints {

    public static RouteGroupBuilder MapPeaksEndpoint(this WebApplication app) {
        var group = app.MapGroup("api/peaks");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapPost("/", CreatePeak);

        return group;
    }

    static async Task<IResult> GetAll(IRepository<Peak> repo, IRepository<Region> regionRepo) {
        var peaks = await repo.GetAllAsync();
        return Results.Ok(peaks);
    }

    static async Task<IResult> GetById(
        int id,
        IRepository<Peak> repo,
        IRepository<Region> regionRepo
    ) {
        var peak = await repo.GetByIDAsync(id);
        var region = await regionRepo.GetByIDAsync(peak.RegionID);
        var peakDto = new PeakDto.NewWithCompleteRegion(peak.Height, peak.Name, region);
        return Results.Ok(peakDto);
    }

    static async Task<IResult> CreatePeak(PeakDto.NewWithRegion peakDto, IRepository<Peak> repo) {
        Peak newPeak = new PeakMapper().MapToEntity(peakDto);

        await repo.AddAsync(newPeak);
        return Results.Ok();
    }

}

