using FgQuota.Api.Contracts;
using FgQuota.Api.Data;
using FgQuota.Api.Exceptions;
using FgQuota.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace FgQuota.Api.Tests;

public class QuotaServiceTests
{
    private static QuotaDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<QuotaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new QuotaDbContext(options);
    }

    [Fact]
    public async Task CreateApplication_ShouldConsumeQuota_AndReturnInQueries()
    {
        await using var context = CreateContext();
        var service = new QuotaService(context, NullLogger<QuotaService>.Instance);

        var response = await service.CreateApplicationAsync(new ApplicationRequest
        {
            EmplId = "E001",
            RegionId = "CN",
            Year = "2026",
            ApplicationType = "SYP",
            StartDt = "2026-02-10",
            EndDt = "2026-02-11",
            DayAmount = 2,
            CreateEmpname = "张三"
        });

        Assert.Equal(1, response.ApplicationId);
        Assert.Equal("审批完成", response.Status);

        var summary = await service.GetQuotaSummaryAsync("E001", "2026");
        var syp = summary.Quotas.Single(q => q.ApplicationType == "SYP");
        Assert.Equal(2, syp.UsedDays);
        Assert.Equal(3, syp.RemainDays);

        var page = await service.GetApplicationsAsync("E001", "2026", null, null, null, 0, 10);
        Assert.Equal(1, page.TotalElements);
        Assert.Single(page.Content);

        var detail = await service.GetApplicationDetailAsync(response.Id);
        Assert.True(detail.HasQuotaDetail);
        Assert.Single(detail.QuotaDetails);
    }

    [Fact]
    public async Task CreateApplication_WhenQuotaInsufficient_ShouldThrow()
    {
        await using var context = CreateContext();
        var service = new QuotaService(context, NullLogger<QuotaService>.Instance);

        await service.CreateApplicationAsync(new ApplicationRequest
        {
            EmplId = "E001",
            RegionId = "CN",
            Year = "2026",
            ApplicationType = "SYP",
            StartDt = "2026-02-10",
            EndDt = "2026-02-14",
            DayAmount = 5,
            CreateEmpname = "张三"
        });

        await Assert.ThrowsAsync<QuotaInsufficientException>(async () =>
        {
            await service.CreateApplicationAsync(new ApplicationRequest
            {
                EmplId = "E001",
                RegionId = "CN",
                Year = "2026",
                ApplicationType = "SYP",
                StartDt = "2026-03-01",
                EndDt = "2026-03-01",
                DayAmount = 1,
                CreateEmpname = "张三"
            });
        });
    }
}
