using AAIA.Shared.Contracts.Extensions;

namespace AAIA.Shared.Contracts.Marketplace;

// ── MoR DTOs (Phase 5.5 — Merchant of Record) ────────────────────────────────

/// <summary>
/// Konfigurierbarer MoR-Anbieter.
/// Wird in appsettings.json unter MerchantOfRecord:Provider gesetzt.
/// </summary>
public enum MorProvider
{
    Disabled,
    LemonSqueezy,
    Paddle
}

/// <summary>
/// Produkt-Mapping: externe MoR-Produkt-ID → AAIA Extension.
/// Wird über eine Admin-API oder direkt in der DB gepflegt.
/// </summary>
public sealed record MorProductMappingDto(
    int              Id,
    string           Provider,
    string           ExternalProductId,
    string?          ExternalVariantId,
    string           ExtensionId,
    AaiaLicenseModel LicenseModel,
    int?             DurationDays,
    bool             IsActive,
    DateTimeOffset   CreatedAt);

/// <summary>
/// Lemon Squeezy Webhook-Event (vereinfachtes Schema für Logging/Audit).
/// </summary>
public sealed record LemonSqueezyEventSummary(
    string  EventName,
    string? OrderId,
    string? SubscriptionId,
    string? BuyerEmail,
    string? ProductId,
    string? VariantId);

/// <summary>
/// Paddle Billing v2 Webhook-Event (vereinfachtes Schema).
/// </summary>
public sealed record PaddleEventSummary(
    string  EventType,
    string? NotificationId,
    string? CustomerId,
    string? BuyerEmail,
    string? ProductId,
    string? PriceId,
    string? SubscriptionId);

/// <summary>
/// Antwort des Webhook-Endpoints (beide Provider).
/// HTTP 200 in allen nicht-Fehler-Fällen (inkl. Skipped, Duplicate).
/// </summary>
public sealed record WebhookAcknowledgeResponse(
    string  Status,       // "Ok" | "Skipped" | "Duplicate"
    string? Action,       // "Created" | "Renewed" | "Revoked" | "Expired" | "WillExpire"
    string? ExtensionId,
    string? Message);

// ── Phase 5.11: MoR Connected Account / Payout Status ────────────────────────

/// <summary>
/// MoR-Verbindungsstatus eines Entwicklers.
/// Enthält ausschließlich abgeleitete Status-Flags — keine Bankdaten, keine KYC-Daten.
/// </summary>
public sealed record MorStatusDto(
    /// <summary>Hat mindestens ein aktives MorProductMapping für eigene Extensions.</summary>
    bool             ProviderConnected,

    /// <summary>"LemonSqueezy" | "Paddle" | null wenn kein Mapping.</summary>
    string?          Provider,

    /// <summary>Checkout ist aktiv (≥1 veröffentlichtes Modul mit CheckoutUrl).</summary>
    bool             CheckoutActive,

    /// <summary>ProviderConnected && CheckoutActive — Payout-Setup vollständig.</summary>
    bool             PayoutSetupComplete,

    /// <summary>Zeitpunkt des letzten verarbeiteten MoR-Webhook-Events.</summary>
    DateTimeOffset?  LastMorEvent,

    /// <summary>
    /// Webhook gilt als gesund wenn:
    ///   - Letztes Event ≤ 30 Tage zurück, ODER
    ///   - Kein Event vorhanden aber Mapping existiert seit ≤ 14 Tagen (noch kein Verkauf erwartet).
    /// </summary>
    bool             WebhookHealthy,

    /// <summary>Anzahl aktiver MorProductMappings für eigene Extensions.</summary>
    int              ActiveMappings,

    /// <summary>Anzahl eigener Module mit gesetzter CheckoutUrl.</summary>
    int              ModulesWithCheckout);

/// <summary>
/// Body für PUT /api/developer/mor/provider.
/// Aktualisiert die CheckoutUrl einer eigenen Extension.
/// </summary>
public sealed record MorProviderUpdateRequest(
    /// <summary>AAIA ExtensionId (muss dem angemeldeten Entwickler gehören).</summary>
    string  ExtensionId,

    /// <summary>
    /// Vollständige Checkout-URL beim MoR-Anbieter.
    /// Null entfernt die URL (deaktiviert Checkout für diese Extension).
    /// </summary>
    string? CheckoutUrl);

/// <summary>
/// MoR-Status eines einzelnen ETW-Kontos (Admin-Sicht).
/// </summary>
public sealed record AdminMorAccountStatusItemDto(
    string           EtwId,
    string           DisplayName,
    bool             HasMorMapping,
    string?          Provider,
    int              TotalExtensions,
    int              ExtensionsWithCheckout,
    int              ActiveMappings,
    DateTimeOffset?  LastWebhookEvent,
    bool             WebhookHealthy,

    /// <summary>"OK" | "NeedsMapping" | "NoCheckout" | "WebhookStale" | "NoExtensions"</summary>
    string           StatusSummary);

/// <summary>
/// Aggregierte MoR-Statusübersicht aller ETW-Konten (Admin-Sicht).
/// Antwort von GET /api/admin/mor/account-status.
/// </summary>
public sealed record AdminMorAccountStatusDto(
    IReadOnlyList<AdminMorAccountStatusItemDto> Items,
    int TotalEtws,
    int EtwsWithMorMapping,
    int EtwsWithCheckout,
    int EtwsWithWebhookIssues);
