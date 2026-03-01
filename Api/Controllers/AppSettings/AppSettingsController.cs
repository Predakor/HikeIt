using Api.Commons.Extentions;
using Application.AppSettings;
using Domain.AppSettings.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.AppSettings;

[Route("api/[controller]")]
[ApiController]
public class AppSettingsController : ControllerBase
{
    private readonly IAppSettingsRepository appSettingsRepository;
    private readonly IAppSettingsService service;

    public AppSettingsController(
        IAppSettingsRepository appSettingsRepository,
        IAppSettingsService service
    )
    {
        this.appSettingsRepository = appSettingsRepository;
        this.service = service;
    }

    [HttpGet]
    public Task<IActionResult> GetAll(CancellationToken ct)
    {
        return appSettingsRepository
            .GetAllAsync(ct)
            .MapAsync(r =>
                r.Select(e => new
                {
                    e.Id,
                    e.Name,
                    Value = e.JsonValue,
                    e.SettingType,
                })
                .ToList()
            )
            .ToActionResultAsync();
    }

    [HttpDelete("{id}")]
    public Task<IActionResult> Remove(int id, CancellationToken ct)
    {
        return service.DeleteSettingAsync(id, ct).ToActionResultAsync();
    }
}
