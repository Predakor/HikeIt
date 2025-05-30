using Application.Services.Files;

namespace Api.Endpoints;

public static class FilesEndpoints {
    public static RouteGroupBuilder MapFilesEndpoints(this WebApplication app) {
        var group = app.MapGroup("api/files");

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //TODO actually add user validation
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        group.MapGet("/{id}", GetById);
        group.MapPost("", AddFile).DisableAntiforgery();
        group.MapDelete("/{id}", DeleteById);

        return group;
    }

    static async Task GetById(int id) {
        throw new NotImplementedException();
    }

    static async Task<IResult> AddFile(IFormFile file, IGpxFileService service) {

        var result = await service.CreateAsync(file);

        return Results.Ok(result.ToString());
    }

    static async Task DeleteById(int id) {
        throw new NotImplementedException();
    }


}
