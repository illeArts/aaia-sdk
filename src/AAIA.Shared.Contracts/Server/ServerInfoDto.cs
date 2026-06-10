using AAIA.Shared.Contracts.Platform;
using AAIA.Shared.Contracts.Versioning;

namespace AAIA.Shared.Contracts.Server;

public sealed record ServerInfoDto(
    string ServerId,
    string Hostname,
    AaiaVersionInfo Version,
    DeviceInfoDto HostDevice,
    string MemoryRoot,
    bool IsConfigured,
    bool IsServiceMode);
