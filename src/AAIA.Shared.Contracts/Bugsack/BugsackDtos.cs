using AAIA.Shared.Contracts.Platform;
using AAIA.Shared.Contracts.Versioning;

namespace AAIA.Shared.Contracts.Bugsack;

public sealed record BugsackReportDto(
    string ReportId,
    AaiaVersionInfo Version,
    DeviceInfoDto Device,
    string Summary,
    string? StackTrace,
    string? LastLogExcerpt,
    IReadOnlyList<string> ActiveModules,
    DateTimeOffset CreatedAt);

public sealed record BugsackFeedbackDto(
    string ClientId,
    string Message,
    string? RelatedReportId = null);
