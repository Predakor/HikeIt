using Application.Services.Files;
using Domain.GpxFiles;

namespace Api.Endpoints;

public static class FilesEndpoints {
    public static RouteGroupBuilder MapFilesEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/files");

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //TODO actually add user validation
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        group.MapGet("", GetAll);
        group.MapGet("/{id}", GetById);

        group.MapPost("", AddFile).DisableAntiforgery();
        group.MapDelete("/{id}", DeleteById);

        return group;
    }

    static async Task<IResult> GetAll(IGpxFileRepository repos) {
        var res = await repos.GetAllAsync();
        return Results.Ok(res);
    }

    static async Task<IResult> GetById(Guid id, IGpxFileRepository repos) {
        var res = await repos.GetGpxFile(id);

        return Results.Ok(res);
    }

    static async Task<IResult> AddFile(IFormFile file, IGpxFileService service) {

        var result = await service.CreateAsync(file);
        if (result == false) {
            return Results.BadRequest("somethign went wrong");
        }
        return Results.Ok(result.ToString());
    }

    static async Task DeleteById(int id) {
        throw new NotImplementedException();
    }


}
