using System.ComponentModel.DataAnnotations;

namespace FgQuota.Api.Contracts;

public sealed class ApplicationRequest
{
    [Required(ErrorMessage = "员工ID不能为空")]
    public string EmplId { get; set; } = string.Empty;

    [Required(ErrorMessage = "区域ID不能为空")]
    public string RegionId { get; set; } = string.Empty;

    [Required(ErrorMessage = "年份不能为空")]
    [RegularExpression("\\d{4}", ErrorMessage = "年份格式错误")]
    public string Year { get; set; } = string.Empty;

    [Required(ErrorMessage = "申请类型不能为空")]
    [RegularExpression("SYP|DSZ|BJH|SZH", ErrorMessage = "申请类型不合法")]
    public string ApplicationType { get; set; } = string.Empty;

    [Required(ErrorMessage = "开始日期不能为空")]
    [RegularExpression("\\d{4}-\\d{2}-\\d{2}", ErrorMessage = "开始日期格式错误")]
    public string StartDt { get; set; } = string.Empty;

    [Required(ErrorMessage = "结束日期不能为空")]
    [RegularExpression("\\d{4}-\\d{2}-\\d{2}", ErrorMessage = "结束日期格式错误")]
    public string EndDt { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "999999", ErrorMessage = "申请天数不能小于0")]
    public decimal? DayAmount { get; set; }

    [Range(typeof(decimal), "0", "999999", ErrorMessage = "申请小时数不能小于0")]
    public decimal? HourAmount { get; set; }

    public string? JoinCertificate { get; set; }

    [Required(ErrorMessage = "申请人姓名不能为空")]
    public string CreateEmpname { get; set; } = string.Empty;
}

public sealed class ApplicationResponse
{
    public string Id { get; set; } = string.Empty;

    public long ApplicationId { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string RdppTaskId { get; set; } = string.Empty;

    public string RdppNodeName { get; set; } = string.Empty;
}

public sealed class ApplicationDetailResponse
{
    public Models.Empvl Application { get; set; } = new();

    public List<Models.EmplQuota> QuotaDetails { get; set; } = new();

    public bool HasQuotaDetail { get; set; }
}

public sealed class RdppCallbackRequest
{
    public string TaskId { get; set; } = string.Empty;

    public string? NodeNumber { get; set; }

    public string? NodeName { get; set; }

    public string? Status { get; set; }

    public string? NextProcessor { get; set; }

    public string? Reviewers { get; set; }

    public string? RdppId { get; set; }

    public string? Comments { get; set; }
}
