using Api.Extentions;
using Application.Commons.FileStorage;
using Application.Services.Auth;
using Application.Services.Files;
using Domain.Common.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase {
    readonly IGpxFileService _fileService;
    readonly IFileStorage _fileStorage;
    readonly IAuthService _authService;

    public FilesController(
        IGpxFileService fileService,
        IFileStorage fileStorage,
        IAuthService authService
    ) {
        _fileService = fileService;
        _fileStorage = fileStorage;
        _authService = authService;
    }

    [HttpGet("demo/trip")]
    public async Task<IActionResult> GetDemoTrip() {
        await Task.CompletedTask;
        var fileUrl = Path.Combine("wwwroot", "demo", "wielka-sowa-trip.gpx");
        var bytes = System.IO.File.ReadAllBytes(fileUrl);

        // 7 days
        Response.Headers.CacheControl = "public,max-age=604800";
        return File(bytes, "application/gpx+xml", "trip-wielka-sowa.gpx");
    }

    [HttpGet("{fileId:guid}")]
    public async Task<IActionResult> GetFile(Guid fileId) {
        return await _authService
            .WithLoggedUserId()
            .BindAsync(_fileStorage.DownloadAsync)
            .ToActionResultAsync();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file) {
        return await _authService
            .WithLoggedUserId()
            .BindAsync(userId => _fileStorage.UploadAsync(file, userId))
            .ToActionResultAsync();
    }
}
