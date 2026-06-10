using AAIA.Shared.Contracts.Platform;

namespace AAIA.Shared.Contracts.Client;

public sealed record ClientPairRequestDto(
    DeviceInfoDto Device,
    string PairingCode,
    string? ClientPublicKey = null);

public sealed record ClientPairResponseDto(
    string ClientId,
    string ServerId,
    string AccessToken,
    DateTimeOffset ExpiresAt,
    LibraryAccessDto LibraryAccess);
