namespace AAIA.Shared.Contracts.Marketplace;

// ── Phase 5.9: ETW Marketplace Dashboard ─────────────────────────────────────
//
// Zeigt dem Entwickler technische Verkaufs- und Lizenzsignale aus MoR-Webhooks.
// KEIN Ersatz für Buchhaltung, Steuerabrechnung oder MoR-Auszahlungen.
// Alle Geldbeträge sind Schätzungen aus Webhook-Daten — nicht abrechnungsrelevant.

// ── Dashboard-Übersicht ───────────────────────────────────────────────────────

/// <summary>
/// GET /api/developer/dashboard — Vollständige Übersicht für den eingeloggten ETW.
/// </summary>
public sealed record DeveloperDashboardDto(
    string                              EtwId,
    string                              DisplayName,
    /// <summary>Anzahl veröffentlichter Extensions (IsPublished = true).</summary>
    int                                 PublishedExtensions,
    /// <summary>Gesamtzahl aktiver Lizenzen über alle Extensions.</summary>
    int                                 TotalActiveLicenses,
    /// <summary>Gesamtzahl erfolgreich verarbeiteter MoR-Webhook-Events (= Käufe laut System).</summary>
    int                                 TotalSalesFromWebhooks,
    /// <summary>Gesamtzahl widerrufener / rückerstatteter Lizenzen.</summary>
    int                                 TotalRevocations,
    /// <summary>Summary pro Extension, sortiert nach Erstellungsdatum (neueste zuerst).</summary>
    IReadOnlyList<DeveloperExtensionSummaryDto> Extensions);

// ── Extension-Summary ─────────────────────────────────────────────────────────

/// <summary>
/// Lizenz- und Verkaufszahlen für eine Extension aus Sicht des Entwicklers.
/// </summary>
public sealed record DeveloperExtensionSummaryDto(
    string          ExtensionId,
    string          Name,
    string          Description,
    /// <summary>"Free" | "Paid" | "Subscription" | "Enterprise"</summary>
    string          LicenseModel,
    /// <summary>"Draft" | "PendingReview" | "MarketplaceVerified" | "Published" | "Rejected"</summary>
    string          Status,
    /// <summary>True wenn neuste Release IsPublished = true.</summary>
    bool            IsPublished,
    string          TrustLevel,
    string?         CheckoutUrl,
    bool            CheckoutActive,
    /// <summary>Provider-Name laut MorProductMapping (z.B. "LemonSqueezy").</summary>
    string?         MorProvider,
    /// <summary>External Product-ID beim MoR-Anbieter.</summary>
    string?         MorExternalProductId,
    // ── Lizenzstatistiken (aus module_licenses) ───────────────────────────────
    int             LicensesTotal,
    int             LicensesActive,
    int             LicensesExpired,
    int             LicensesRevoked,
    int             LicensesUnclaimed,
    // ── Verkaufssignale (aus mor_webhook_events) ──────────────────────────────
    /// <summary>Anzahl "Created"-Events = neue Käufe laut Webhooks.</summary>
    int             SalesFromWebhooks,
    /// <summary>Anzahl "Revoked"-Events laut Webhooks.</summary>
    int             RevocationsFromWebhooks,
    string?         LastWebhookEventType,
    DateTimeOffset? LastWebhookAt,
    DateTimeOffset  CreatedAt);

// ── Sales Summary (Detailansicht) ─────────────────────────────────────────────

/// <summary>
/// GET /api/developer/extensions/{id}/sales-summary — Aufgeschlüsselte Verkaufsdaten.
/// </summary>
public sealed record ExtensionSalesSummaryDto(
    string          ExtensionId,
    string          Name,
    /// <summary>Käufe pro Monat (letzten 12 Monate, aus Webhook-Events).</summary>
    IReadOnlyList<MonthlySalesEntry> MonthlySales,
    int             TotalSales,
    int             TotalRevocations,
    int             NetLicenses,
    /// <summary>
    /// Geschätzter Bruttoumsatz in EUR (aus Webhook-Daten, falls AmountPaid bekannt).
    /// NICHT abrechnungsrelevant — nur Richtwert.
    /// </summary>
    decimal?        EstimatedGrossRevenue,
    /// <summary>Hinweis: "Beträge sind Schätzungen aus Webhooks. Verbindliche Abrechnung erfolgt über {Provider}."</summary>
    string          DisclaimerNote);

/// <summary>Monatliche Verkaufszahl (für Sparkline / Tabelle).</summary>
public sealed record MonthlySalesEntry(
    /// <summary>Format: "2026-05" (yyyy-MM)</summary>
    string YearMonth,
    int    Sales,
    int    Revocations);

// ── Lizenzliste (paginiert) ───────────────────────────────────────────────────

/// <summary>
/// GET /api/developer/extensions/{id}/licenses — Lizenzliste (Entwickler-Sicht).
/// Käufer-Email ist aus Datenschutzgründen teilweise anonymisiert: a***@domain.com.
/// </summary>
public sealed record DeveloperLicenseEntryDto(
    int             LicenseId,
    /// <summary>Anonymisierte Käufer-Email (z.B. "a***@gmail.com").</summary>
    string          BuyerEmailMasked,
    string          Status,
    string          ClaimStatus,
    DateTimeOffset  IssuedAt,
    DateTimeOffset? ExpiresAt,
    DateTimeOffset? ClaimedAt);

public sealed record DeveloperLicensesPageDto(
    string                              ExtensionId,
    int                                 TotalCount,
    int                                 Page,
    int                                 PageSize,
    IReadOnlyList<DeveloperLicenseEntryDto> Items);

// ── Webhook-Events (Debugansicht) ─────────────────────────────────────────────

/// <summary>
/// GET /api/developer/extensions/{id}/webhook-events — Letzte MoR-Events.
/// </summary>
public sealed record DeveloperWebhookEventDto(
    int             Id,
    string          Provider,
    string          EventType,
    string          LicenseAction,
    DateTimeOffset  ProcessedAt,
    /// <summary>Käufer-Email (anonymisiert).</summary>
    string?         BuyerEmailMasked);
