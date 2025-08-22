using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Files;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase {
    [HttpGet("demo/trip")]
    public async Task<IActionResult> GetDemoTrip() {
        await Task.CompletedTask;
        var fileUrl = Path.Combine("wwwroot", "demo", "wielka-sowa-trip.gpx");
        var bytes = System.IO.File.ReadAllBytes(fileUrl);

        // 7 days
        Response.Headers.CacheControl = "public,max-age=604800";
        return File(bytes, "application/gpx+xml", "trip-wielka-sowa.gpx");
    }
}
