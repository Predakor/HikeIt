using HikeIt.Api.Dto;
using HikeIt.Api.Entities;
using HikeIt.Api.Repository;

namespace HikeIt.Api.Endpoints;

public static class PeaksEndpoints {
    public static RouteGroupBuilder MapPeaksEndpoint(this WebApplication app) {
        var group = app.MapGroup("peaks");

        group.MapGet("/", GetAll);
        group.MapGet("/{id}", GetById);
        group.MapPost("/", CreatePeak);

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

    static async Task<IResult> CreatePeak(PeakDto.NewWithRegion peakDto, IRepository<Peak> repo) {
        Peak newPeak = DtoToEntity(peakDto);

        await repo.AddAsync(newPeak);
        return Results.Ok();
    }

    private static Peak DtoToEntity(PeakDto peak) {
        Peak newPeak;
        switch (peak) {
            case PeakDto.New newDTo:
                newPeak = new Peak() { Height = newDTo.Height, Name = newDTo.Name, RegionID = 1 };
                break;
            default:
                newPeak = new Peak();
                break;
        }

        return newPeak;
    }
}
