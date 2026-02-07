namespace FakeQuotaSystem.App;

public class FakeDataStore
{
    private readonly List<EmplQuota> _quotas;
    private readonly List<Empvl> _employees;
    private readonly List<ApprovalItem> _approvals;
    private long _nextQuotaId;
    private long _nextEmployeeId;
    private long _nextApprovalId;

    public FakeDataStore()
    {
        _quotas = new List<EmplQuota>
        {
            new()
            {
                Id = 1,
                RegionId = "BJ",
                QuotaSeqNo = 1,
                Year = 2026,
                ApplicationType = "北京",
                DayAmount = 365,
                HourAmount = 8760,
                QuotaDayAmount = 365,
                QuotaHourAmount = 525600,
                Remarks = "北京年度额度"
            },
            new()
            {
                Id = 2,
                RegionId = "SZ",
                QuotaSeqNo = 1,
                Year = 2026,
                ApplicationType = "深圳",
                DayAmount = 365,
                HourAmount = 8760,
                QuotaDayAmount = 365,
                QuotaHourAmount = 525600,
                Remarks = "深圳年度额度"
            }
        };

        _employees = new List<Empvl>
        {
            new()
            {
                Id = 1,
                EmpId = "EMP001",
                EmployeeName = "张三",
                ActivityName = "陪护假(5天)",
                ActivityDay = "5",
                Certificate = "CERT-001",
                Status = "Active"
            },
            new()
            {
                Id = 2,
                EmpId = "EMP002",
                EmployeeName = "李四",
                ActivityName = "陪护假(7天)",
                ActivityDay = "7",
                Certificate = "CERT-002",
                Status = "Active"
            }
        };

        _approvals = new List<ApprovalItem>
        {
            new()
            {
                ApprovalId = 1,
                ApprovalType = "LeaveApplication",
                Operation = "Create",
                ApplicantId = "EMP001",
                ApplicantName = "张三",
                RequestType = "陪护假",
                StartDate = "2026-01-10",
                EndDate = "2026-01-14",
                RequestedDays = 5,
                Attachment = "CERT-001",
                Subject = "陪护假申请 5 天"
            }
        };

        _nextQuotaId = _quotas.Max(x => x.Id) + 1;
        _nextEmployeeId = _employees.Max(x => x.Id) + 1;
        _nextApprovalId = _approvals.Max(x => x.ApprovalId) + 1;
    }

    public IReadOnlyList<EmplQuota> GetQuotas() =>
        _quotas.OrderBy(x => x.RegionId).ThenBy(x => x.QuotaSeqNo).ToList();

    public EmplQuota? GetQuota(long id) => _quotas.FirstOrDefault(x => x.Id == id);

    public ApprovalItem SubmitQuotaApproval(
        string operation,
        EmplQuota quotaPayload,
        string applicantId,
        string applicantName)
    {
        var payload = CloneQuota(quotaPayload);
        var approval = new ApprovalItem
        {
            ApprovalId = _nextApprovalId++,
            ApprovalType = "QuotaChange",
            Operation = operation,
            ApplicantId = applicantId,
            ApplicantName = applicantName,
            RequestType = "额度变更",
            StartDate = "-",
            EndDate = "-",
            RequestedDays = 0,
            Attachment = "-",
            Subject = BuildQuotaSubject(operation, payload),
            QuotaPayload = payload
        };

        _approvals.Add(approval);
        return approval;
    }

    public IReadOnlyList<Empvl> GetEmployees() => _employees;

    public IReadOnlyList<Empvl> GetActiveEmployees() =>
        _employees.Where(x => x.Status == "Active").ToList();

    public Empvl? GetEmployee(long id) => _employees.FirstOrDefault(x => x.Id == id);

    public Empvl AddEmployee(Empvl employee)
    {
        employee.Id = _nextEmployeeId++;
        employee.CreateDate = DateTime.UtcNow;
        employee.UpdateDate = null;
        _employees.Add(employee);
        return employee;
    }

    public bool UpdateEmployee(long id, Empvl update)
    {
        var current = GetEmployee(id);
        if (current is null)
        {
            return false;
        }

        current.EmployeeName = update.EmployeeName;
        current.ActivityName = update.ActivityName;
        current.ActivityDay = update.ActivityDay;
        current.Certificate = update.Certificate;
        current.Status = update.Status;
        current.UpdateDate = DateTime.UtcNow;
        return true;
    }

    public bool DeleteEmployee(long id)
    {
        var current = GetEmployee(id);
        if (current is null)
        {
            return false;
        }

        current.Status = "Deleted";
        current.UpdateDate = DateTime.UtcNow;
        return true;
    }

    public ApprovalItem SubmitLeaveApplication(NewApplicationRequest request)
    {
        var applicant = _employees.FirstOrDefault(x => x.EmpId == request.EmployeeId);
        var applicantName = string.IsNullOrWhiteSpace(request.ApplicantName)
            ? applicant?.EmployeeName ?? "未知员工"
            : request.ApplicantName;
        var requestType = string.IsNullOrWhiteSpace(request.RequestType) ? "假勤申请" : request.RequestType.Trim();
        var requestedDays = request.RequestedDays > 0 ? request.RequestedDays : request.Days;
        var startDate = string.IsNullOrWhiteSpace(request.StartDate) ? "-" : request.StartDate.Trim();
        var endDate = string.IsNullOrWhiteSpace(request.EndDate) ? "-" : request.EndDate.Trim();
        var attachment = string.IsNullOrWhiteSpace(request.Attachment) ? "-" : request.Attachment.Trim();

        var approval = new ApprovalItem
        {
            ApprovalId = _nextApprovalId++,
            ApprovalType = "LeaveApplication",
            Operation = "Create",
            ApplicantId = request.EmployeeId,
            ApplicantName = applicantName,
            RequestType = requestType,
            StartDate = startDate,
            EndDate = endDate,
            RequestedDays = requestedDays,
            Attachment = attachment,
            Subject = $"{requestType} 申请 {requestedDays} 天"
        };

        _approvals.Add(approval);
        return approval;
    }

    public IReadOnlyList<ApprovalItem> GetPendingApprovals() =>
        _approvals
            .Where(x => x.Status == "PendingHrApproval")
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

    public bool ReviewApproval(long approvalId, bool approved, string hrApprover)
    {
        var current = _approvals.FirstOrDefault(x => x.ApprovalId == approvalId);
        if (current is null || current.Status != "PendingHrApproval")
        {
            return false;
        }

        current.HrApprover = hrApprover;
        current.ReviewedAt = DateTime.UtcNow;
        current.Status = approved ? "Approved" : "Rejected";

        if (approved && current.ApprovalType == "QuotaChange" && current.QuotaPayload is not null)
        {
            ApplyQuotaChange(current.Operation, current.QuotaPayload);
        }

        return true;
    }

    private void ApplyQuotaChange(string operation, EmplQuota payload)
    {
        if (operation.Equals("Create", StringComparison.OrdinalIgnoreCase))
        {
            payload.Id = _nextQuotaId++;
            _quotas.Add(CloneQuota(payload));
            return;
        }

        var current = GetQuota(payload.Id);
        if (current is null)
        {
            return;
        }

        if (operation.Equals("Delete", StringComparison.OrdinalIgnoreCase))
        {
            _quotas.Remove(current);
            return;
        }

        current.RegionId = payload.RegionId;
        current.QuotaSeqNo = payload.QuotaSeqNo;
        current.Year = payload.Year;
        current.ApplicationType = payload.ApplicationType;
        current.DayAmount = payload.DayAmount;
        current.HourAmount = payload.HourAmount;
        current.QuotaDayAmount = payload.QuotaDayAmount;
        current.QuotaHourAmount = payload.QuotaHourAmount;
        current.Remarks = payload.Remarks;
    }

    private static EmplQuota CloneQuota(EmplQuota source) =>
        new()
        {
            Id = source.Id,
            RegionId = source.RegionId,
            QuotaSeqNo = source.QuotaSeqNo,
            Year = source.Year,
            ApplicationType = source.ApplicationType,
            DayAmount = source.DayAmount,
            HourAmount = source.HourAmount,
            QuotaDayAmount = source.QuotaDayAmount,
            QuotaHourAmount = source.QuotaHourAmount,
            Remarks = source.Remarks
        };

    private static string BuildQuotaSubject(string operation, EmplQuota quota)
    {
        var op = operation switch
        {
            "Create" => "新增",
            "Update" => "修改",
            "Delete" => "删除",
            _ => operation
        };

        return $"{op}额度: {quota.RegionId}-{quota.ApplicationType}-{quota.Year}";
    }
}
