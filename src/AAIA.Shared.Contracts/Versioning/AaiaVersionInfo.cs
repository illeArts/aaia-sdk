namespace AAIA.Shared.Contracts.Versioning;

public sealed record AaiaVersionInfo(
    string ProductVersion,
    string ContractVersion,
    AaiaReleaseChannel Channel,
    string? BuildMetadata = null,
    DateTimeOffset? BuiltAt = null);
