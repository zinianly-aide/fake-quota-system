using FgQuota.Api.Contracts;
using FgQuota.Api.Data;
using FgQuota.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FgQuota.Api.Tests;

public class RdppServiceTests
{
    private static QuotaDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<QuotaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new QuotaDbContext(options);
    }

    [Fact]
    public async Task HandleCallback_ShouldUpdateStatus()
    {
        await using var context = CreateContext();
        var quotaService = new QuotaService(context, NullLogger<QuotaService>.Instance);
        var rdppService = new RdppService(context, NullLogger<RdppService>.Instance);

        var created = await quotaService.CreateApplicationAsync(new ApplicationRequest
        {
            EmplId = "E100",
            RegionId = "CN",
            Year = "2026",
            ApplicationType = "DSZ",
            StartDt = "2026-04-01",
            EndDt = "2026-04-01",
            DayAmount = 1,
            CreateEmpname = "李四"
        });

        var app = (await quotaService.GetApplicationDetailAsync(created.Id)).Application;

        await rdppService.HandleCallbackAsync(new RdppCallbackRequest
        {
            TaskId = app.RdpptaskId!,
            Status = "rejected",
            NodeName = "部门主管审批",
            NodeNumber = "2"
        });

        var updated = (await quotaService.GetApplicationDetailAsync(created.Id)).Application;
        Assert.Equal("已驳回", updated.Status);
        Assert.Equal("2", updated.RdppnodeNumber);
    }
}
