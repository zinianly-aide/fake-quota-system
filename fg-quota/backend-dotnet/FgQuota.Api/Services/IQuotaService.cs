using FgQuota.Api.Contracts;
using FgQuota.Api.Models;

namespace FgQuota.Api.Services;

public interface IQuotaService
{
    Task<QuotaSummaryResponse> GetQuotaSummaryAsync(string emplId, string year);

    Task<bool> CheckQuotaAvailableAsync(string emplId, string year, string type, decimal applyDays);

    Task<decimal> GetRemainingQuotaAsync(string emplId, string year, string type);

    Task<ApplicationResponse> CreateApplicationAsync(ApplicationRequest request);

    Task<PageResponse<Empvl>> GetApplicationsAsync(string? emplId, string? year, string? type, string? status, string? keyword, int page, int size);

    Task<ApplicationDetailResponse> GetApplicationDetailAsync(string id);

    Task<List<Empvl>> GetRecentApplicationsAsync(string emplId, int limit);

    object GetQuotaConfigs();
}
