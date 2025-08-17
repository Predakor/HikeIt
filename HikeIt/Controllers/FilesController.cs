using Application.Services.Files;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase {
    readonly IGpxFileService _fileService;

    public FilesController(IGpxFileService fileService) {
        _fileService = fileService;
    }

    [HttpGet("demo/trip")]
    public IActionResult GetDemoTrip() {
        var fileUrl = Path.Combine("wwwroot", "demo", "wielka-sowa-trip.gpx");
        var bytes = System.IO.File.ReadAllBytes(fileUrl);

        // 7 days
        Response.Headers.CacheControl = "public,max-age=604800";

        return File(bytes, "application/gpx+xml", "trip-wielka-sowa.gpx");
    }
}
