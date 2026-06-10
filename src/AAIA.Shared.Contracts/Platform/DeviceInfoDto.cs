namespace AAIA.Shared.Contracts.Platform;

public sealed record DeviceInfoDto(
    string DeviceId,
    string DisplayName,
    AaiaOperatingSystem OperatingSystem,
    AaiaDeviceKind DeviceKind,
    AaiaCpuArchitecture Architecture,
    IReadOnlyList<PlatformCapability> Capabilities);
