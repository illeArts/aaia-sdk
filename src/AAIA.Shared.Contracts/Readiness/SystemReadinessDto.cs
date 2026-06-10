namespace AAIA.Shared.Contracts.Readiness;

public sealed record SystemReadinessDto(
    DetectedHardwareDto Hardware,
    SystemSuitability Suitability,
    int Score,
    string Summary,
    IReadOnlyList<string> Reasons,
    IReadOnlyList<string> Recommendations,
    IReadOnlyList<ModelAssessmentDto> ModelAssessments);
