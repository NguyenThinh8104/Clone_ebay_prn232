namespace BE.Common;

public class ApiResponse<T>
{
    public bool Success { get; set; } = true;
    public T? Data { get; set; }
    public ApiError? Error { get; set; }
    public ApiMeta Meta { get; set; } = new();

    public static ApiResponse<T> SuccessResult(T data, string? traceId = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Meta = new ApiMeta { TraceId = traceId }
        };
    }

    public static ApiResponse<T> ErrorResult(string code, string message, string? traceId = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Error = new ApiError { Code = code, Message = message },
            Meta = new ApiMeta { TraceId = traceId }
        };
    }
}

public class ApiError
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<string> Details { get; set; } = new();
}

public class ApiMeta
{
    public string? TraceId { get; set; }
}
