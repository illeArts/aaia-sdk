namespace AAIA.Shared.Contracts.Readiness;

public sealed record ModelRequirementDto(
    string ModelId,
    string DisplayName,
    string ModelKind,
    long? MinimumMemoryBytes,
    long? RecommendedMemoryBytes,
    long? MinimumGpuMemoryBytes,
    long? RecommendedGpuMemoryBytes,
    bool RequiresGpu,
    bool SupportsCpuOnly);
