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
