namespace FgQuota.Api.Exceptions;

public abstract class ApiException : Exception
{
    protected ApiException(int status, string error, string message) : base(message)
    {
        Status = status;
        Error = error;
    }

    public int Status { get; }

    public string Error { get; }
}

public sealed class BadRequestException : ApiException
{
    public BadRequestException(string message) : base(StatusCodes.Status400BadRequest, "参数校验失败", message)
    {
    }
}

public sealed class ResourceNotFoundException : ApiException
{
    public ResourceNotFoundException(string message) : base(StatusCodes.Status404NotFound, "资源不存在", message)
    {
    }
}

public sealed class QuotaInsufficientException : ApiException
{
    public QuotaInsufficientException(string message) : base(StatusCodes.Status409Conflict, "额度不足", message)
    {
    }
}
