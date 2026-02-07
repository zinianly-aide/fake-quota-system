using FgQuota.Api.Contracts;
using FgQuota.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FgQuota.Api.Controllers;

[ApiController]
[Route("api/quota")]
public sealed class QuotaController : ControllerBase
{
    private readonly IQuotaService _quotaService;

    public QuotaController(IQuotaService quotaService)
    {
        _quotaService = quotaService;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetQuotaSummary([FromQuery] string emplId, [FromQuery] string year)
    {
        return Ok(await _quotaService.GetQuotaSummaryAsync(emplId, year));
    }

    [HttpGet("applications")]
    public async Task<IActionResult> GetApplications(
        [FromQuery] string? emplId,
        [FromQuery] string? year,
        [FromQuery] string? type,
        [FromQuery] string? status,
        [FromQuery] string? keyword,
        [FromQuery] int page = 0,
        [FromQuery] int size = 10)
    {
        return Ok(await _quotaService.GetApplicationsAsync(emplId, year, type, status, keyword, page, size));
    }

    [HttpGet("applications/recent")]
    public async Task<IActionResult> GetRecentApplications([FromQuery] string emplId, [FromQuery] int limit = 5)
    {
        return Ok(await _quotaService.GetRecentApplicationsAsync(emplId, limit));
    }

    [HttpGet("applications/{id}")]
    public async Task<IActionResult> GetApplicationDetail(string id)
    {
        return Ok(await _quotaService.GetApplicationDetailAsync(id));
    }

    [HttpPost("applications")]
    public async Task<IActionResult> CreateApplication([FromBody] ApplicationRequest request)
    {
        return Ok(await _quotaService.CreateApplicationAsync(request));
    }

    [HttpGet("configs")]
    public IActionResult GetQuotaConfigs()
    {
        return Ok(_quotaService.GetQuotaConfigs());
    }

    [HttpGet("check")]
    public async Task<IActionResult> CheckQuota([FromQuery] string emplId, [FromQuery] string year, [FromQuery] string type)
    {
        var remain = await _quotaService.GetRemainingQuotaAsync(emplId, year, type);
        return Ok(new
        {
            remainDays = remain,
            sufficient = remain > 0
        });
    }
}
