namespace AAIA.Shared.Contracts.Marketplace;

// ── Phase 5.10: Owner/Admin Marketplace Console ────────────────────────────────
//
// Betreiber-Sicht: Gesamtstatistiken, Risk/Trust, Webhook-Health, Review-Queue.
// Kein Finanzsystem — nur technische Plattformüberwachung.

// ── Gesamtübersicht ───────────────────────────────────────────────────────────

/// <summary>GET /api/admin/marketplace/overview</summary>
public sealed record MarketplaceOverviewDto(
    // Extensions
    int TotalExtensions,
    int PublishedExtensions,
    int PendingReviewCount,
    int BlockedReleasesCount,
    /// <summary>Extensions mit RiskLevel "High" oder "Medium".</summary>
    int HighRiskExtensions,
    int MediumRiskExtensions,

    // ETW-Entwickler
    int TotalDevelopers,
    int VerifiedDevelopers,
    int PendingDevelopers,

    // Lizenzen
    int TotalLicenses,
    int ActiveLicenses,
    int ExpiredLicenses,
    int RevokedLicenses,
    int UnclaimedLicenses,

    // MoR-Webhooks (letzte 24h)
    int WebhookEventsLast24h,
    int WebhookSalesLast24h,
    int WebhookRevocationsLast24h,

    // Käuferkonten
    int TotalBuyerAccounts,

    DateTimeOffset GeneratedAt);

// ── Extension-Liste (Admin) ───────────────────────────────────────────────────

/// <summary>Eintrag in der Admin-Extension-Liste.</summary>
public sealed record AdminExtensionDto(
    string          ExtensionId,
    string          Name,
    string          PublisherEtwId,
    string          PublisherName,
    string          LicenseModel,
    string          Status,
    bool            IsPublished,
    string?         LatestVersion,
    string?         TrustLevel,
    string?         RiskLevel,
    bool            CheckoutActive,
    string?         MorProvider,
    int             ActiveLicenses,
    int             TotalLicenses,
    int             SalesFromWebhooks,
    DateTimeOffset  CreatedAt,
    DateTimeOffset? LastWebhookAt);

/// <summary>Detail-Ansicht einer Extension (inkl. Releases-Liste).</summary>
public sealed record AdminExtensionDetailDto(
    AdminExtensionDto                    Summary,
    IReadOnlyList<AdminReleaseEntryDto>  Releases,
    IReadOnlyList<DeveloperLicenseEntryDto> RecentLicenses);

/// <summary>Release-Zeile in der Admin-Ansicht.</summary>
public sealed record AdminReleaseEntryDto(
    int             ReleaseId,
    string          Version,
    bool            IsPublished,
    bool            IsLatest,
    string?         TrustLevel,
    string?         RiskLevel,
    bool            IsBlocked,
    DateTimeOffset  PublishedAt,
    DateTimeOffset? VisibleSinceAt);

// ── ETW-Entwickler (Admin) ────────────────────────────────────────────────────

/// <summary>Zeile in der Admin-ETW-Liste.</summary>
public sealed record AdminDeveloperDto(
    string          EtwId,
    string          DisplayName,
    string          Email,
    string          Role,
    string          Status,
    bool            Verified,
    bool            TotpEnabled,
    int             ModuleCount,
    int             PublishedModules,
    int             TotalActiveLicenses,
    DateTimeOffset  CreatedAt,
    DateTimeOffset? LastLoginAt);

/// <summary>Detail-Ansicht eines ETWs.</summary>
public sealed record AdminDeveloperDetailDto(
    AdminDeveloperDto                      Summary,
    IReadOnlyList<AdminExtensionDto>       Extensions);

// ── Lizenzen (Admin) ─────────────────────────────────────────────────────────

/// <summary>Admin-Lizenz-Eintrag (vollständige E-Mail, nicht maskiert).</summary>
public sealed record AdminLicenseDto(
    int             LicenseId,
    string          ExtensionId,
    string          ExtensionName,
    string          PublisherEtwId,
    string          BuyerEmail,
    string          LicenseKey,
    string          LicenseModel,
    string          Status,
    string          ClaimStatus,
    DateTimeOffset  IssuedAt,
    DateTimeOffset? ExpiresAt,
    DateTimeOffset? ClaimedAt,
    string?         TransactionId);

public sealed record AdminLicensesPageDto(
    int                              TotalCount,
    int                              Page,
    int                              PageSize,
    IReadOnlyList<AdminLicenseDto>   Items);

// ── Pending-Review-Queue ──────────────────────────────────────────────────────

/// <summary>Release das auf manuelle Freigabe wartet.</summary>
public sealed record PendingReviewReleaseDto(
    int             ReleaseId,
    string          ExtensionId,
    string          ExtensionName,
    string          PublisherEtwId,
    string          PublisherName,
    string          Version,
    string?         TrustLevel,
    string?         RiskLevel,
    string?         RequestedPermissions,
    DateTimeOffset  SubmittedAt,
    /// <summary>Tage seit Einreichung (Priorisierung).</summary>
    int             DaysPending);

// ── Block / Unblock ───────────────────────────────────────────────────────────

public sealed record BlockReleaseRequest(
    /// <summary>Grund der Sperrung (für Audit-Log + ETW-Benachrichtigung).</summary>
    string Reason);

public sealed record BlockReleaseResponse(
    bool   Success,
    string Message,
    int    ReleaseId,
    bool   IsBlocked);
