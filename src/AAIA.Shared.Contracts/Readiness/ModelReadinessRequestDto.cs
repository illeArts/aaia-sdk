namespace AAIA.Shared.Contracts.Readiness;

public sealed record ModelReadinessRequestDto(
    IReadOnlyList<SelectedModelDto> SelectedModels,
    int ExpectedConcurrentClients = 1,
    bool PreferLocalModels = true);
