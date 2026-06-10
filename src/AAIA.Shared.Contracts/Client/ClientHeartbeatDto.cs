using AAIA.Shared.Contracts.Platform;

namespace AAIA.Shared.Contracts.Client;

public sealed record ClientHeartbeatDto(
    string ClientId,
    DeviceInfoDto? Device,
    DateTimeOffset SentAt);
