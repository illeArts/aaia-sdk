namespace AAIA.Shared.Contracts.Common;

public sealed record AaiaApiError(
    string Code,
    string Message,
    string? Detail = null,
    string? TraceId = null);
