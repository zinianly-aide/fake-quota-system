using Microsoft.AspNetCore.Mvc;

namespace FakeQuotaSystem.App.Controllers;

[ApiController]
[Route("api/application")]
public class ApplicationController : ControllerBase
{
    private readonly FakeDataStore _store;
    private readonly string _attachmentRoot;
    private static readonly HashSet<string> AllowedAttachmentExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf",
        ".png",
        ".jpg",
        ".jpeg",
        ".doc",
        ".docx",
        ".txt"
    };
    private const long MaxAttachmentBytes = 10 * 1024 * 1024;

    public ApplicationController(FakeDataStore store, IWebHostEnvironment environment)
    {
        _store = store;
        _attachmentRoot = Path.Combine(environment.ContentRootPath, "uploads");
        Directory.CreateDirectory(_attachmentRoot);
    }

    [HttpGet("summary")]
    public IActionResult Summary()
    {
        var employees = _store.GetEmployees();
        var quotas = _store.GetQuotas();
        var pending = _store.GetPendingApprovals();

        return Ok(new
        {
            totalEmployees = employees.Count,
            totalQuotaTypes = quotas.Count,
            pendingApprovals = pending.Count
        });
    }

    [HttpGet("pending")]
    public IActionResult Pending() => Ok(_store.GetPendingApprovals());

    [HttpPost("new")]
    public IActionResult Create([FromBody] NewApplicationRequest request)
    {
        var approval = _store.SubmitLeaveApplication(request);
        return Ok(new
        {
            status = "PendingApproval",
            approvalId = approval.ApprovalId
        });
    }

    [HttpPost("upload")]
    [RequestSizeLimit(MaxAttachmentBytes)]
    public async Task<IActionResult> UploadAttachment([FromForm] IFormFile? file)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest(new { message = "请先选择附件文件。" });
        }

        if (file.Length > MaxAttachmentBytes)
        {
            return BadRequest(new { message = "附件大小不能超过 10MB。" });
        }

        var extension = Path.GetExtension(file.FileName);
        if (!AllowedAttachmentExtensions.Contains(extension))
        {
            return BadRequest(new { message = "仅支持 pdf/png/jpg/jpeg/doc/docx/txt 格式。" });
        }

        var storedFileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(_attachmentRoot, storedFileName);

        await using (var stream = System.IO.File.Create(fullPath))
        {
            await file.CopyToAsync(stream);
        }

        var attachmentUrl = $"{Request.Scheme}://{Request.Host}/api/application/attachment/{Uri.EscapeDataString(storedFileName)}";
        return Ok(new
        {
            attachment = attachmentUrl,
            fileName = file.FileName
        });
    }

    [HttpGet("attachment/{fileName}")]
    public IActionResult GetAttachment(string fileName)
    {
        var normalized = Path.GetFileName(fileName);
        if (!string.Equals(fileName, normalized, StringComparison.Ordinal))
        {
            return BadRequest();
        }

        var fullPath = Path.Combine(_attachmentRoot, normalized);
        if (!System.IO.File.Exists(fullPath))
        {
            return NotFound();
        }

        return PhysicalFile(fullPath, "application/octet-stream", enableRangeProcessing: true);
    }

    [HttpPost("approve/{applicationId:long}")]
    public IActionResult Approve(long applicationId, [FromBody] ApproveApplicationRequest request)
    {
        return _store.ReviewApproval(applicationId, request.Approved, request.HrApprover) ? NoContent() : NotFound();
    }

    [HttpPost("update-usage/{quotaId:long}")]
    public IActionResult UpdateUsage(long quotaId, [FromBody] UsageUpdateRequest request)
    {
        return Ok(new
        {
            quotaId,
            usedAmount = request.UsedAmount,
            updatedAt = DateTime.UtcNow
        });
    }
}
