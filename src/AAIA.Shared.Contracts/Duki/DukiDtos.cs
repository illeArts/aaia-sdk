namespace AAIA.Shared.Contracts.Duki;

public enum DukiActionStatus
{
    Pending,
    Approved,
    Denied,
    WaitingForClient,
    Executed,
    Failed,
    Expired
}

/// <summary>
/// Einheitliche DUKI-Risikostufen (Green-Skala).
/// Ordinal: höher = gefährlicher.
/// Mapping vom alten Schema: Low→Green, Medium→Yellow, High→Orange, Critical→Red.
/// </summary>
public enum DukiRiskLevel
{
    /// <summary>Sofort ausführbar, kein Protokoll erforderlich.</summary>
    Green = 0,

    /// <summary>Sofort ausführbar, Audit-Pflicht.</summary>
    Yellow = 1,

    /// <summary>Admin-Freigabe erforderlich.</summary>
    Orange = 2,

    /// <summary>SuperAdmin + starke Authentifizierung erforderlich.</summary>
    Red = 3,

    /// <summary>Grundsätzlich verboten. Keine Ausnahme.</summary>
    Black = 4
}

public static class DukiRiskLevelExtensions
{
    /// <summary>Konvertiert altes Low/Medium/High/Critical-Schema auf Green-Skala.</summary>
    public static DukiRiskLevel FromLegacyString(string legacy) => legacy.ToLowerInvariant() switch
    {
        "low"      => DukiRiskLevel.Green,
        "medium"   => DukiRiskLevel.Yellow,
        "high"     => DukiRiskLevel.Orange,
        "critical" => DukiRiskLevel.Red,
        "green"    => DukiRiskLevel.Green,
        "yellow"   => DukiRiskLevel.Yellow,
        "orange"   => DukiRiskLevel.Orange,
        "red"      => DukiRiskLevel.Red,
        "black"    => DukiRiskLevel.Black,
        _          => DukiRiskLevel.Orange
    };

    public static bool RequiresApproval(this DukiRiskLevel r) => r >= DukiRiskLevel.Orange;
    public static bool RequiresBiometric(this DukiRiskLevel r) => r >= DukiRiskLevel.Red;
    public static bool IsBlocked(this DukiRiskLevel r)        => r == DukiRiskLevel.Black;
    public static bool AuditRequired(this DukiRiskLevel r)    => r >= DukiRiskLevel.Yellow;
}

public sealed record DukiActionDto(
    string Id,
    string Title,
    string Description,
    DukiRiskLevel Risk,
    DukiActionStatus Status,
    string RequestedBy,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt);

public sealed record DukiActionTransitionRequestDto(
    string TargetStatus,
    string? Reason = null,
    string? ClientId = null);

public sealed record DukiMissionRequestDto(
    string MissionType,
    string Goal,
    IReadOnlyDictionary<string, string>? Parameters = null);

public sealed record DukiMissionStepDto(
    string Id,
    string Title,
    string Mode,
    DukiRiskLevel Risk,
    string Status);

public sealed record DukiMissionPlanDto(
    string MissionId,
    string MissionType,
    string Goal,
    IReadOnlyList<DukiMissionStepDto> Steps);
