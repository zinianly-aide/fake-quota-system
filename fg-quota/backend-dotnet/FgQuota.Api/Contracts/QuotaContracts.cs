namespace FgQuota.Api.Contracts;

public sealed class QuotaSummaryDto
{
    public string ApplicationType { get; set; } = string.Empty;

    public string TypeName { get; set; } = string.Empty;

    public decimal TotalDays { get; set; }

    public decimal UsedDays { get; set; }

    public decimal RemainDays { get; set; }

    public decimal UsagePercent { get; set; }
}

public sealed class QuotaSummaryResponse
{
    public string Year { get; set; } = string.Empty;

    public string EmplId { get; set; } = string.Empty;

    public List<QuotaSummaryDto> Quotas { get; set; } = new();
}

public sealed class PageResponse<T>
{
    public List<T> Content { get; set; } = new();

    public long TotalElements { get; set; }

    public int TotalPages { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }
}
