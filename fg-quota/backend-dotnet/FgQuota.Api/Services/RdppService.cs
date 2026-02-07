using FgQuota.Api.Contracts;
using FgQuota.Api.Data;
using FgQuota.Api.Exceptions;
using FgQuota.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FgQuota.Api.Services;

public sealed class RdppService : IRdppService
{
    private readonly QuotaDbContext _dbContext;
    private readonly ILogger<RdppService> _logger;

    public RdppService(QuotaDbContext dbContext, ILogger<RdppService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task HandleCallbackAsync(RdppCallbackRequest request)
    {
        var app = await _dbContext.Applications.FirstOrDefaultAsync(x => x.RdpptaskId == request.TaskId);
        if (app is null)
        {
            throw new ResourceNotFoundException("流程任务不存在");
        }

        app.RdppnodeNumber = request.NodeNumber;
        app.RdppnodeName = request.NodeName;
        app.Rdppreviewers = request.Reviewers;
        app.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        var callbackStatus = request.Status?.Trim().ToLowerInvariant();
        if (callbackStatus == "approved")
        {
            app.Status = "已通过";
            await EnsureQuotaDetailAsync(app);
        }
        else if (callbackStatus == "rejected")
        {
            app.Status = "已驳回";
        }
        else
        {
            app.Status = string.IsNullOrWhiteSpace(request.NodeName) ? "审批中" : request.NodeName;
        }

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("RDPP callback handled for task {TaskId}", request.TaskId);
    }

    public async Task MockAdvanceAsync(string taskId, string action)
    {
        var callback = new RdppCallbackRequest
        {
            TaskId = taskId,
            Status = string.Equals(action, "approved", StringComparison.OrdinalIgnoreCase) ? "approved" : "rejected",
            NodeName = "审批完成",
            NodeNumber = "99"
        };

        await HandleCallbackAsync(callback);
    }

    private async Task EnsureQuotaDetailAsync(Empvl app)
    {
        if (await _dbContext.QuotaDetails.AnyAsync(x => x.ApplicationId == app.ApplicationId))
        {
            return;
        }

        var dayAmount = app.DayAmount ?? app.ApplyQuotaDays;
        var hourAmount = app.HourAmount ?? dayAmount * QuotaConstants.HoursPerDay;

        var lastSeq = await _dbContext.QuotaDetails.MaxAsync(x => (long?)x.QuotaSeqno) ?? 0;
        var quota = new EmplQuota
        {
            QuotaSeqno = lastSeq + 1,
            RegionId = string.IsNullOrWhiteSpace(app.RegionId) ? "CN" : app.RegionId,
            EmplId = app.Empid,
            Year = app.ActivityDay,
            ApplicationType = app.ApplyType,
            ApplicationId = app.ApplicationId,
            StartDt = app.StartDt,
            EndDt = app.EndDt,
            DayAmount = dayAmount,
            HourAmount = hourAmount,
            QuotaDayAmount = QuotaConstants.TypeQuotaMap.GetValueOrDefault(app.ApplyType, 0),
            QuotaHourAmount = QuotaConstants.TypeQuotaMap.GetValueOrDefault(app.ApplyType, 0) * QuotaConstants.HoursPerDay
        };

        _dbContext.QuotaDetails.Add(quota);
    }
}
