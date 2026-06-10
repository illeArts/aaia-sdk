using AAIA.Shared.Contracts.Platform;

namespace AAIA.Shared.Contracts.Readiness;

public sealed record DetectedHardwareDto(
    AaiaOperatingSystem OperatingSystem,
    AaiaCpuArchitecture Architecture,
    string? CpuModel,
    int? CpuCores,
    int? CpuThreads,
    long? MemoryBytes,
    string? GpuModel,
    long? GpuMemoryBytes,
    string? NpuModel,
    long? FreeStorageBytes,
    bool? IsAdminOrRoot,
    bool? IsBatteryPowered,
    bool? HasNetwork,
    IReadOnlyList<string> DetectedRuntimes);
