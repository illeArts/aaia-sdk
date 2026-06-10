namespace AAIA.Shared.Contracts.Readiness;

public sealed record SelectedModelDto(
    string ModelId,
    string DisplayName,
    string ModelKind,
    int ExpectedConcurrentSessions = 1);
