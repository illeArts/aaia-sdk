namespace AAIA.Shared.Contracts.Discovery;

public sealed record ServerDiscoveryResultDto(
    IReadOnlyList<DiscoveredServerDto> Servers,
    DateTimeOffset ScannedAt);
