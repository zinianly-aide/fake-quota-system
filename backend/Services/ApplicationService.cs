using Microsoft.EntityFrameworkCore;
using FakeQuotaSystem.Models;
using FakeQuotaSystem.Data;
using FakeQuotaSystem.Models;
using Serilog;
using System.Text.Json;

namespace FakeQuotaSystem.Services
{
    public interface IApplicationService
    {
        Task<EmplvlSummary> GetEmployeeSummaryAsync();
        Task<EmplQuotaSummary> GetQuotaSummaryAsync();
        Task<string> CreateNewApplicationAsync(NewApplicationRequest request);
        Task<bool> ApproveApplicationAsync(long applicationId, bool approved);
        Task<string> UpdateQuotaUsageAsync(long quotaId, decimal usedAmount);
        Task<List<ApplicationStatus>> GetPendingApprovalsAsync();
    }

    public record NewApplicationRequest
    {
        public string RegionId { get; init; }
        public string EmployeeId { get; init; }
        public int Days { get; init; }
        public string Remarks { get; init; }
    }

    public record ApplicationStatus
    {
        public long ApplicationId { get; init; }
        public string EmployeeId { get; init; }
        public string EmployeeName { get; init; }
        public string ActivityName { get; init; }
        public int Days { get; init; }
        public decimal AppliedQuota { get; init; }
        public string Status { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? ApprovedAt { get; init; }
    }

    public class ApplicationService : IApplicationService
    {
        private readonly QuotaDbContext _context;

        public ApplicationService(QuotaDbContext context)
        {
            _context = context;
        }

        public async Task<EmplvlSummary> GetEmployeeSummaryAsync()
        {
            var employees = await _context.Empvls
                .Where(e => e.Status != "Deleted")
                .GroupBy(e => new
                {
                    e.EmpId,
                    e.EmpId,
                    e.ActivityName
                })
                .Select(g => new EmplvlSummary
                {
                    TotalEmployees = g.Count(),
                    TotalCertificates = g.Count(e => e.Certificate != null)
                })
                .ToListAsync();

            Log.Information("Retrieved employee summary: {TotalEmployees} employees", employees.Sum(e => e.TotalEmployees));

            return employees.First();
        }

        public async Task<EmplQuotaSummary> GetQuotaSummaryAsync()
        {
            var quotas = await _context.EmplQuotas
                .GroupBy(q => new
                {
                    q.RegionId
                })
                .Select(g => new EmplQuotaSummary
                {
                    TotalRegions = g.Count(),
                    TotalEmpvlTypes = g.Count(),
                    TotalApplications = g.Count() * 10, // Mock calculation
                    ActiveApplications = (int)(g.Count() * 0.85)   // Mock calculation
                    PendingApprovals = (int)(g.Count() * 0.15)   // Mock calculation
                })
                .ToListAsync();

            Log.Information("Retrieved quota summary: {TotalRegions} regions", quotas.Sum(q => q.TotalApplications));

            return quotas.First();
        }

        public async Task<string> CreateNewApplicationAsync(NewApplicationRequest request)
        {
            Log.Information("Creating new application: Region={request.RegionId}, Employee={request.EmployeeId}, Days={request.Days}");
            Log.Information("Application requires approval before quota is allocated");

            var application = $"APP-{Guid.NewGuid():N}";
            Log.Information($"Application created with ID: {application}. Status: PendingApproval");
            
            return application;
        }

        public async Task<bool> ApproveApplicationAsync(long applicationId, bool approved)
        {
            Log.Information("Approving application: {applicationId}, Approved: {approved}");
            
            // Mock approval logic
            await Task.Delay(100); // Simulate processing
            
            if (approved)
            {
                Log.Information("Application approved. Quota will be allocated.");
                return true;
            }
            else
            {
                Log.Warning("Application rejected.");
                return false;
            }
        }

        public async Task<string> UpdateQuotaUsageAsync(long quotaId, decimal usedAmount)
        {
            Log.Information($"Updating quota usage: QuotaId={quotaId}, UsedAmount={usedAmount}");
            
            // Mock usage update
            var newUsage = usedAmount + (decimal)(new Random().Next(1000, 10000));
            
            Log.Information($"New quota usage recorded: {newUsage}");
            
            return $"Usage updated successfully. New total: {newUsage}";
        }

        public async Task<List<ApplicationStatus>> GetPendingApprovalsAsync()
        {
            Log.Information("Retrieving pending approvals...");
            
            // Mock pending approvals
            var pendingApprovals = new List<ApplicationStatus>
            {
                new ApplicationStatus
                {
                    ApplicationId = 1,
                    EmployeeId = "EMP001",
                    EmployeeName = "张三",
                    ActivityName = "北京-年度假",
                    Days = 5,
                    AppliedQuota = 5,
                    Status = "PendingApproval",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new ApplicationStatus
                {
                    ApplicationId = 2,
                    EmployeeId = "EMP001",
                    EmployeeName = "张三",
                    ActivityName = "深圳-年度假",
                    Days = 7,
                    AppliedQuota = 7,
                    Status = "PendingApproval",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            };

            return pendingApprovals;
        }
    }
}
