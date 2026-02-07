using FgQuota.Api.Contracts;
using FgQuota.Api.Data;
using FgQuota.Api.Exceptions;
using FgQuota.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FgQuota.Api.Services;

public sealed class QuotaService : IQuotaService
{
    private readonly QuotaDbContext _dbContext;
    private readonly ILogger<QuotaService> _logger;

    public QuotaService(QuotaDbContext dbContext, ILogger<QuotaService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<QuotaSummaryResponse> GetQuotaSummaryAsync(string emplId, string year)
    {
        var details = await _dbContext.QuotaDetails
            .AsNoTracking()
            .Where(x => x.EmplId == emplId && x.Year == year)
            .ToListAsync();

        var usedByType = details
            .GroupBy(x => x.ApplicationType, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.DayAmount ?? 0), StringComparer.OrdinalIgnoreCase);

        var quotas = QuotaConstants.TypeQuotaMap.Select(type =>
        {
            var usedDays = usedByType.GetValueOrDefault(type.Key, 0);
            var remainDays = type.Value - usedDays;
            var usagePercent = type.Value == 0 ? 0 : Math.Round(Math.Clamp(usedDays * 100 / type.Value, 0, 100), 2);

            return new QuotaSummaryDto
            {
                ApplicationType = type.Key,
                TypeName = QuotaConstants.TypeNameMap[type.Key],
                TotalDays = type.Value,
                UsedDays = usedDays,
                RemainDays = remainDays,
                UsagePercent = usagePercent
            };
        }).ToList();

        return new QuotaSummaryResponse
        {
            Year = year,
            EmplId = emplId,
            Quotas = quotas
        };
    }

    public async Task<bool> CheckQuotaAvailableAsync(string emplId, string year, string type, decimal applyDays)
    {
        var remain = await GetRemainingQuotaAsync(emplId, year, type);
        return remain >= applyDays;
    }

    public async Task<decimal> GetRemainingQuotaAsync(string emplId, string year, string type)
    {
        var total = QuotaConstants.TypeQuotaMap.GetValueOrDefault(type, 0);
        var used = await GetUsedDaysInternalAsync(emplId, year, type);
        return total - used;
    }

    public async Task<ApplicationResponse> CreateApplicationAsync(ApplicationRequest request)
    {
        if (!QuotaConstants.TypeQuotaMap.ContainsKey(request.ApplicationType))
        {
            throw new BadRequestException("申请类型不合法");
        }

        if (!DateOnly.TryParse(request.StartDt, out var startDt) || !DateOnly.TryParse(request.EndDt, out var endDt))
        {
            throw new BadRequestException("开始日期或结束日期格式错误");
        }

        if (endDt < startDt)
        {
            throw new BadRequestException("结束日期不能早于开始日期");
        }

        var dayAmount = request.DayAmount ?? 0;
        var hourAmount = request.HourAmount ?? 0;
        var applyDays = dayAmount + hourAmount / QuotaConstants.HoursPerDay;

        if (applyDays <= 0)
        {
            throw new BadRequestException("申请天数或小时数必须大于0");
        }

        if (!await CheckQuotaAvailableAsync(request.EmplId, request.Year, request.ApplicationType, applyDays))
        {
            var remain = await GetRemainingQuotaAsync(request.EmplId, request.Year, request.ApplicationType);
            throw new QuotaInsufficientException($"额度不足，剩余额度：{remain}天");
        }

        var applicationId = await NextApplicationIdAsync();
        var now = DateTime.Now;
        var taskId = BuildTaskId();

        var app = new Empvl
        {
            Id = Guid.NewGuid().ToString("N"),
            Empid = request.EmplId,
            ActivityName = QuotaConstants.TypeNameMap[request.ApplicationType],
            ActivityDay = request.Year,
            RegionId = request.RegionId,
            JoinCertificate = request.JoinCertificate,
            Status = "审批完成",
            CreateEmpid = request.EmplId,
            CreateEmpname = request.CreateEmpname,
            CreateDate = now.ToString("yyyy-MM-dd HH:mm:ss"),
            ApplyQuotaDays = Math.Round(applyDays, 4),
            ApplyType = request.ApplicationType,
            ApplicationId = applicationId,
            StartDt = request.StartDt,
            EndDt = request.EndDt,
            DayAmount = request.DayAmount,
            HourAmount = request.HourAmount,
            RdpptaskId = taskId,
            RdppnodeNumber = "99",
            RdppnodeAccount = "hr001",
            RdppnodeName = "审批完成",
            Rdppid = taskId
        };

        _dbContext.Applications.Add(app);

        await EnsureQuotaDetailAsync(app);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Application {ApplicationId} created for {EmployeeId}", app.ApplicationId, app.Empid);

        return new ApplicationResponse
        {
            Id = app.Id,
            ApplicationId = app.ApplicationId,
            Status = app.Status,
            Message = "申请创建成功",
            RdppTaskId = app.RdpptaskId ?? string.Empty,
            RdppNodeName = app.RdppnodeName ?? string.Empty
        };
    }

    public async Task<PageResponse<Empvl>> GetApplicationsAsync(string? emplId, string? year, string? type, string? status, string? keyword, int page, int size)
    {
        var safePage = Math.Max(page, 0);
        var safeSize = Math.Clamp(size, 1, 100);

        var query = _dbContext.Applications.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(emplId))
        {
            query = query.Where(x => x.Empid == emplId);
        }

        if (!string.IsNullOrWhiteSpace(year))
        {
            query = query.Where(x => x.ActivityDay == year);
        }

        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(x => x.ApplyType == type);
        }

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => (x.Empid != null && x.Empid.Contains(keyword))
                                     || (x.CreateEmpname != null && x.CreateEmpname.Contains(keyword)));
        }

        var total = await query.LongCountAsync();
        var content = await query.OrderByDescending(x => x.CreateDate)
            .Skip(safePage * safeSize)
            .Take(safeSize)
            .ToListAsync();

        return new PageResponse<Empvl>
        {
            Content = content,
            TotalElements = total,
            TotalPages = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safeSize),
            CurrentPage = safePage,
            PageSize = safeSize
        };
    }

    public async Task<ApplicationDetailResponse> GetApplicationDetailAsync(string id)
    {
        var app = await _dbContext.Applications.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (app is null)
        {
            throw new ResourceNotFoundException("申请记录不存在");
        }

        var details = await _dbContext.QuotaDetails.AsNoTracking()
            .Where(x => x.ApplicationId == app.ApplicationId)
            .OrderByDescending(x => x.QuotaSeqno)
            .ToListAsync();

        if (details.Count == 0 && (app.Status == "已通过" || app.Status == "审批完成"))
        {
            details = await _dbContext.QuotaDetails.AsNoTracking()
                .Where(x => x.EmplId == app.Empid && x.Year == app.ActivityDay && x.ApplicationType == app.ApplyType)
                .OrderByDescending(x => x.QuotaSeqno)
                .ToListAsync();
        }

        return new ApplicationDetailResponse
        {
            Application = app,
            QuotaDetails = details,
            HasQuotaDetail = details.Count > 0
        };
    }

    public async Task<List<Empvl>> GetRecentApplicationsAsync(string emplId, int limit)
    {
        var safeLimit = Math.Clamp(limit, 1, 20);
        return await _dbContext.Applications.AsNoTracking()
            .Where(x => x.Empid == emplId)
            .OrderByDescending(x => x.CreateDate)
            .Take(safeLimit)
            .ToListAsync();
    }

    public object GetQuotaConfigs()
    {
        return new
        {
            types = QuotaConstants.TypeNameMap,
            quotas = QuotaConstants.TypeQuotaMap
        };
    }

    private async Task<decimal> GetUsedDaysInternalAsync(string emplId, string year, string type)
    {
        var sum = await _dbContext.QuotaDetails
            .AsNoTracking()
            .Where(x => x.EmplId == emplId && x.Year == year && x.ApplicationType.Equals(type, StringComparison.OrdinalIgnoreCase))
            .SumAsync(x => (decimal?)(x.DayAmount ?? 0));

        return sum ?? 0;
    }

    private async Task<long> NextApplicationIdAsync()
    {
        var last = await _dbContext.Applications.MaxAsync(x => (long?)x.ApplicationId) ?? 0;
        return last + 1;
    }

    private async Task<long> NextQuotaSeqAsync()
    {
        var last = await _dbContext.QuotaDetails.MaxAsync(x => (long?)x.QuotaSeqno) ?? 0;
        return last + 1;
    }

    private async Task EnsureQuotaDetailAsync(Empvl app)
    {
        if (await _dbContext.QuotaDetails.AnyAsync(x => x.ApplicationId == app.ApplicationId))
        {
            return;
        }

        var dayAmount = app.DayAmount ?? app.ApplyQuotaDays;
        var hourAmount = app.HourAmount ?? dayAmount * QuotaConstants.HoursPerDay;

        var quota = new EmplQuota
        {
            QuotaSeqno = await NextQuotaSeqAsync(),
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

    private static string BuildTaskId()
    {
        return $"TASK_{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";
    }
}
