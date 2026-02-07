using FgQuota.Api.Contracts;
using FgQuota.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FgQuota.Api.Controllers;

[ApiController]
[Route("api/rdpp")]
public sealed class RdppController : ControllerBase
{
    private readonly IRdppService _rdppService;

    public RdppController(IRdppService rdppService)
    {
        _rdppService = rdppService;
    }

    [HttpPost("callback")]
    public async Task<IActionResult> HandleCallback([FromBody] RdppCallbackRequest request)
    {
        await _rdppService.HandleCallbackAsync(request);
        return Ok(new { code = "SUCCESS", message = "回调处理成功" });
    }

    [HttpPost("mock/advance")]
    public async Task<IActionResult> MockAdvance([FromQuery] string taskId, [FromQuery] string action)
    {
        await _rdppService.MockAdvanceAsync(taskId, action);
        return Ok(new { code = "SUCCESS", message = "模拟流程推进成功" });
    }
}
