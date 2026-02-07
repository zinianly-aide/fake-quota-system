namespace FgQuota.Api.Models;

public sealed class Empvl
{
    public string Id { get; set; } = string.Empty;

    public string Empid { get; set; } = string.Empty;

    public string ActivityName { get; set; } = string.Empty;

    public string ActivityDay { get; set; } = string.Empty;

    public string RegionId { get; set; } = string.Empty;

    public string? JoinCertificate { get; set; }

    public string Status { get; set; } = string.Empty;

    public string CreateEmpid { get; set; } = string.Empty;

    public string CreateEmpname { get; set; } = string.Empty;

    public string CreateDate { get; set; } = string.Empty;

    public string? UpdateEmpid { get; set; }

    public string? UpdateEmpname { get; set; }

    public string? UpdateDate { get; set; }

    public decimal ApplyQuotaDays { get; set; }

    public long ApplicationId { get; set; }

    public string StartDt { get; set; } = string.Empty;

    public string EndDt { get; set; } = string.Empty;

    public decimal? DayAmount { get; set; }

    public decimal? HourAmount { get; set; }

    public string? RdpptaskId { get; set; }

    public string? RdppnodeNumber { get; set; }

    public string? RdppnodeAccount { get; set; }

    public string? RdppnodeName { get; set; }

    public string? Rdppreviewers { get; set; }

    public string? Rdppid { get; set; }

    public string ApplyType { get; set; } = string.Empty;
}
