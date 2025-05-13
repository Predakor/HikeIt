using HikeIt.Api.Data;
using HikeIt.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace HikeIt.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionController(TripDbContext context) : ControllerBase {
    readonly TripDbContext _dbContext = context;
    DbSet<Region> Regions => _dbContext.Regions;

    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var results = await Regions.ToListAsync();
        return ApiResponseResolver.Resolve(results);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        var result = await Regions.FindAsync(id);
        return ApiResponseResolver.Resolve(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Region region) {
        Region newRegion = new() { Name = region.Name };

        if (newRegion == null) {
            return BadRequest();
        }

        await Regions.AddAsync(newRegion);
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}

public static class ApiResponseResolver {
    public static IActionResult Resolve<T>(T? result) {
        if (result is null) {
            return new NotFoundResult();
        }

        if (result is IEnumerable collection and not string) {
            var enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext()) {
                return new NotFoundResult();
            }
        }

        return new OkObjectResult(result);
    }
}
