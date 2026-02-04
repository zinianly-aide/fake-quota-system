using FakeQuotaSystem.Models;
using Serilog;

namespace FakeQuotaSystem.Services
{
    public interface IEmplTypeService
    {
        Task<IEnumerable<EmplQuota>> GetAllEmplTypesAsync();
        Task<EmplQuota> GetEmplTypeByIdAsync(int id);
        Task<EmplQuota> CreateEmplTypeAsync(EmplTypeCreateDto dto);
        Task<EmplQuota> UpdateEmplTypeAsync(int id, EmplTypeUpdateDto dto);
        Task<EmplQuota> DeleteEmplTypeAsync(int id);
    }

    public interface IEmplQuotaService : IEmplTypeService
    {
        Task<IEnumerable<EmplQuota>> GetEmplQuotasAsync();
        Task<EmplQuota> GetEmplQuotaByIdAsync(int id);
        Task<EmplQuota> CreateEmplQuotaAsync(EmplQuotaCreateDto dto);
        Task<EmplQuota> UpdateEmplQuotaAsync(int id, EmplQuotaUpdateDto dto);
        Task<EmplQuota> DeleteEmplQuotaAsync(int id);
    }
}
