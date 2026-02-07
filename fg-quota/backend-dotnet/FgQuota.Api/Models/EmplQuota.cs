namespace FgQuota.Api.Models;

public sealed class EmplQuota
{
    public long Id { get; set; }

    public string RegionId { get; set; } = "CN";

    public long QuotaSeqno { get; set; }

    public string EmplId { get; set; } = string.Empty;

    public string Year { get; set; } = string.Empty;

    public string ApplicationType { get; set; } = string.Empty;

    public long ApplicationId { get; set; }

    public string? StartDt { get; set; }

    public string? EndDt { get; set; }

    public decimal? DayAmount { get; set; }

    public decimal? HourAmount { get; set; }

    public decimal QuotaDayAmount { get; set; }

    public decimal QuotaHourAmount { get; set; }
}
