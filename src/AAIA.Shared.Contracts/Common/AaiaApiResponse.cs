namespace AAIA.Shared.Contracts.Common;

public sealed record AaiaApiResponse<T>(
    bool Success,
    T? Data,
    AaiaApiError? Error = null)
{
    public static AaiaApiResponse<T> Ok(T data) => new(true, data);

    public static AaiaApiResponse<T> Fail(string code, string message, string? detail = null, string? traceId = null) =>
        new(false, default, new AaiaApiError(code, message, detail, traceId));
}
