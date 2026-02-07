namespace FakeQuotaSystem.App;

public class EmplQuota
{
    public long Id { get; set; }
    public string RegionId { get; set; } = string.Empty;
    public int QuotaSeqNo { get; set; }
    public int Year { get; set; }
    public string ApplicationType { get; set; } = string.Empty;
    public decimal DayAmount { get; set; }
    public decimal HourAmount { get; set; }
    public decimal QuotaDayAmount { get; set; }
    public decimal QuotaHourAmount { get; set; }
    public string Remarks { get; set; } = string.Empty;
}

public class Empvl
{
    public long Id { get; set; }
    public string EmpId { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string ActivityName { get; set; } = string.Empty;
    public string ActivityDay { get; set; } = string.Empty;
    public string? Certificate { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDate { get; set; }
}

public class PendingApplication
{
    public long ApplicationId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string EmployeeName { get; set; } = string.Empty;
    public string ActivityName { get; set; } = string.Empty;
    public int Days { get; set; }
    public decimal AppliedQuota { get; set; }
    public string Status { get; set; } = "PendingApproval";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ApprovalItem
{
    public long ApprovalId { get; set; }
    public string ApprovalType { get; set; } = string.Empty; // LeaveApplication | QuotaChange
    public string Operation { get; set; } = string.Empty; // Create | Update | Delete
    public string ApplicantId { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public decimal RequestedDays { get; set; }
    public string Attachment { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Status { get; set; } = "PendingHrApproval";
    public string? HrApprover { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReviewedAt { get; set; }
    public EmplQuota? QuotaPayload { get; set; }
}

public class NewApplicationRequest
{
    public string RegionId { get; set; } = string.Empty; // Compatibility field
    public string EmployeeId { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public decimal RequestedDays { get; set; }
    public string Attachment { get; set; } = string.Empty;
    public int Days { get; set; } // Compatibility field
    public string Remarks { get; set; } = string.Empty;
}

public class ApproveApplicationRequest
{
    public bool Approved { get; set; }
    public string HrApprover { get; set; } = "HR001";
}

public class UsageUpdateRequest
{
    public decimal UsedAmount { get; set; }
}
