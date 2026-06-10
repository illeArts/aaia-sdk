using AAIA.Shared.Contracts.Platform;
using AAIA.Shared.Contracts.Versioning;

namespace AAIA.Shared.Contracts.Discovery;

public sealed record DiscoveredServerDto(
    string ServerId,
    string DisplayName,
    string BaseUrl,
    AaiaOperatingSystem OperatingSystem,
    AaiaVersionInfo Version,
    bool IsConfigured,
    bool RequiresPairing,
    int? LatencyMs = null);
