namespace Aaia.Shared.Contracts.Evolution;

// ── Enums ─────────────────────────────────────────────────────────────────────

/// <summary>
/// Autonomiestufe der Evolution Engine.
/// Stufe 5 (FullAutonomy) ist per Direktive gesperrt.
/// </summary>
public enum EvolutionStage
{
    /// <summary>Stufe 0 — Nur manuelle Beobachtung, keine automatischen Aktionen.</summary>
    ManualOnly      = 0,
    /// <summary>Stufe 1 — Automatische Beobachtung im Hintergrund, keine Aktionen.</summary>
    AutoObserve     = 1,
    /// <summary>Stufe 2 — Automatische Tests in Sandbox, keine Produktivänderungen.</summary>
    AutoTest        = 2,
    /// <summary>Stufe 3 — Automatische Migrationsvorschläge und Patchpläne.</summary>
    AutoPropose     = 3,
    /// <summary>Stufe 4 — Teilautomatische Evolution, nur nach Freigabe durch Eigentümer.</summary>
    SemiAuto        = 4,
    /// <summary>Stufe 5 — Vollständige Selbstanpassung. PER DIREKTIVE GESPERRT.</summary>
    FullAutonomy    = 5
}

/// <summary>Büro der Evolution Engine, das die Beobachtung erzeugt hat.</summary>
public enum EvolutionOffice
{
    TechnologyWatch  = 0,
    AiBenchmark      = 1,
    Security         = 2,
    Compatibility    = 3,
    ScientificResearch = 4,
    MarketObservation  = 5,
    EvolutionPlanning  = 6,
    CivilizationImpact = 7
}

/// <summary>Relevanzeinschätzung einer Beobachtung.</summary>
public enum EvolutionRelevance
{
    Low    = 0,
    Medium = 1,
    High   = 2,
    /// <summary>Kritisch — sofortige Analyse erforderlich (z. B. API-Abkündigung &lt; 60 Tage).</summary>
    Critical = 3
}

/// <summary>Aktueller Status einer Technologie im Tracking.</summary>
public enum TechnologyStatus
{
    /// <summary>Beobachten — noch keine Handlungsnotwendigkeit.</summary>
    Watching   = 0,
    /// <summary>Analysieren — konkreter Einfluss auf AAIA wird geprüft.</summary>
    Analyzing  = 1,
    /// <summary>Handeln — Migration oder Anpassung empfohlen.</summary>
    ActionRequired = 2,
    /// <summary>Abgeschlossen — Entscheidung getroffen, Plan umgesetzt.</summary>
    Resolved   = 3,
    /// <summary>Ignoriert — bewusst als nicht relevant eingestuft.</summary>
    Dismissed  = 4
}

/// <summary>Entscheidungsebene eines Evolutionsplans.</summary>
public enum EvolutionPlanAction
{
    Observe   = 0,
    Analyze   = 1,
    Recommend = 2,
    Plan      = 3
}

// ── DTOs ──────────────────────────────────────────────────────────────────────

/// <summary>Rohe Beobachtung aus einem Office.</summary>
public sealed record EvolutionObservationDto(
    string          Id,
    EvolutionOffice Office,
    string          Title,
    string          Summary,
    EvolutionRelevance Relevance,
    string?         SourceUrl,
    string          ObservedAt,
    bool            IsProcessed
);

/// <summary>Technologie oder KI-Modell im Tracking.</summary>
public sealed record EvolutionTechnologyDto(
    string           Id,
    string           Name,
    string           Category,       // "ai_model" | "framework" | "os" | "hardware" | ...
    string           Vendor,
    string?          Version,
    TechnologyStatus Status,
    EvolutionRelevance Relevance,
    string?          Notes,
    string           FirstSeenAt,
    string           UpdatedAt
);

/// <summary>Kompatibilitätsereignis — z. B. API-Abkündigung.</summary>
public sealed record EvolutionCompatibilityEventDto(
    string  Id,
    string  Title,
    string  AffectedComponent,  // z. B. "Google Maps API v2"
    string  ChangeType,         // "deprecation" | "breaking_change" | "eol"
    string? DeadlineDate,       // ISO-8601, wenn bekannt
    int     DaysUntilDeadline,  // -1 wenn unbekannt
    EvolutionRelevance Urgency,
    string? MigrationNotes,
    string  DetectedAt,
    bool    IsResolved
);

/// <summary>Evolutionsplan — Empfehlung des Planning Office.</summary>
public sealed record EvolutionPlanDto(
    string             Id,
    string             Title,
    string             Description,
    EvolutionPlanAction RecommendedAction,
    EvolutionRelevance  Priority,
    string[]           AffectedModules,
    string?            TriggerObservationId,
    string             CreatedAt,
    string?            ApprovedAt,
    string?            ApprovedBy,
    bool               IsApproved
);

/// <summary>Aktueller Status der Evolution Engine.</summary>
public sealed record EvolutionStatusDto(
    EvolutionStage   CurrentStage,
    bool             IsBackgroundRunning,
    string?          LastRunAt,
    int              TotalObservations,
    int              PendingPlans,
    int              CriticalEvents,
    string           DirectiveNote   // Hinweis auf Stufe-5-Verbot
);

/// <summary>Anfrage zum manuellen Anlegen einer Beobachtung (Stage 0).</summary>
public sealed record CreateObservationRequest(
    EvolutionOffice    Office,
    string             Title,
    string             Summary,
    EvolutionRelevance Relevance,
    string?            SourceUrl = null
);

/// <summary>Anfrage zum Aktualisieren des Evolution-Stage.</summary>
public sealed record SetEvolutionStageRequest(
    EvolutionStage Stage
);
