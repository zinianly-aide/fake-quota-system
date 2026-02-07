using Microsoft.AspNetCore.Mvc;

namespace FakeQuotaSystem.App.Controllers;

[ApiController]
[Route("api/emplquota")]
public class EmplQuotaController : ControllerBase
{
    private readonly FakeDataStore _store;

    public EmplQuotaController(FakeDataStore store)
    {
        _store = store;
    }

    [HttpGet("all")]
    public IActionResult GetAll() => Ok(_store.GetQuotas());

    [HttpGet("{id:long}")]
    public IActionResult GetById(long id)
    {
        var quota = _store.GetQuota(id);
        return quota is null ? NotFound() : Ok(quota);
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] EmplQuota quota)
    {
        var approval = _store.SubmitQuotaApproval(
            "Create",
            quota,
            applicantId: "EMP_SUBMITTER",
            applicantName: "业务提交人");

        return Accepted(new
        {
            status = "PendingHrApproval",
            approvalId = approval.ApprovalId,
            message = "额度新增申请已提交，待 HR 签核后生效。"
        });
    }

    [HttpPut("{id:long}")]
    public IActionResult Update(long id, [FromBody] EmplQuota quota)
    {
        var existing = _store.GetQuota(id);
        if (existing is null)
        {
            return NotFound();
        }

        quota.Id = id;
        var approval = _store.SubmitQuotaApproval(
            "Update",
            quota,
            applicantId: "EMP_SUBMITTER",
            applicantName: "业务提交人");

        return Accepted(new
        {
            status = "PendingHrApproval",
            approvalId = approval.ApprovalId,
            message = "额度修改申请已提交，待 HR 签核后生效。"
        });
    }

    [HttpDelete("{id:long}")]
    public IActionResult Delete(long id)
    {
        var existing = _store.GetQuota(id);
        if (existing is null)
        {
            return NotFound();
        }

        var approval = _store.SubmitQuotaApproval(
            "Delete",
            existing,
            applicantId: "EMP_SUBMITTER",
            applicantName: "业务提交人");

        return Accepted(new
        {
            status = "PendingHrApproval",
            approvalId = approval.ApprovalId,
            message = "额度删除申请已提交，待 HR 签核后生效。"
        });
    }
}
