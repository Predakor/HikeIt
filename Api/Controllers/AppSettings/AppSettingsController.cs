using Api.Commons.Extentions;
using Application.AppSettings;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Controllers.AppSettings;

[Route("api/[controller]")]
[ApiController]
public class AppSettingsController : ControllerBase
{
    private readonly IAppSettingsService service;

    public AppSettingsController(IAppSettingsService service)
    {
        this.service = service;
    }

    [HttpGet]
    public Task<IActionResult> GetAll(CancellationToken ct)
    {
        return service.GetAllAsync(ct).ToActionResultAsync();
    }

    [HttpDelete("{id}")]
    public Task<IActionResult> Remove(int id, CancellationToken ct)
    {
        return service.DeleteSettingAsync(id, ct).ToActionResultAsync();
    }

    [HttpPut("{id}")]
    public Task<IActionResult> Update(int id, JsonDocument jsonSetting, CancellationToken ct)
    {
        return service.Update(id, jsonSetting, ct).ToActionResultAsync();
    }
}
