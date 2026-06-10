using AAIA.Shared.Contracts.Versioning;

namespace AAIA.Shared.Contracts.Client;

public sealed record ClientConfigDto(
    string ClientId,
    AaiaVersionInfo Version,
    string PreferredServerUrl,
    string? RemoteServerUrl,
    bool BugsackEnabled,
    IReadOnlyList<string> EnabledFeatureFlags);
