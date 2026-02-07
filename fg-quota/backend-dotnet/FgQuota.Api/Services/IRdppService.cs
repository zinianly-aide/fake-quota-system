using FgQuota.Api.Contracts;

namespace FgQuota.Api.Services;

public interface IRdppService
{
    Task HandleCallbackAsync(RdppCallbackRequest request);

    Task MockAdvanceAsync(string taskId, string action);
}
