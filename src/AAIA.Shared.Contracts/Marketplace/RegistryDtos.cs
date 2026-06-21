namespace AAIA.Shared.Contracts.Marketplace;

// ── Registry DTOs (Phase 5.2) ─────────────────────────────────────────────────
//
// Öffentliche Sicht auf den Marketplace.
// Wird von RegistryController, Module Manager und AAIAS-Client konsumiert.
// Enthält NUR was öffentlich sichtbar sein darf (kein PublicKeyPem, keine DB-IDs).

/// <summary>
/// Kurzinfo zu einer Extension im öffentlichen Marketplace-Katalog.
/// Rückgabe von GET /api/marketplace/extensions.
/// </summary>
public sealed record RegistryExtensionDto(
    string  ExtensionId,
    string  DisplayName,
    string? Description,
    string? Category,
    string? IconUrl,
    string  PublisherEtwId,
    string  PublisherDisplayName,
    string  ExtensionType,          // "Module" | "Plugin" | "LanguagePack"
    string  LicenseModel,           // "Free" | "Paid" | "Subscription" | "Enterprise" | "Trial"
    decimal? Price,
    string?  Currency,
    /// <summary>
    /// Direktlink zur Checkout-Seite des MoR-Anbieters (Lemon Squeezy / Paddle).
    /// Null bei Free-Extensions oder wenn noch nicht konfiguriert.
    /// WooCommerce und Module Manager nutzen diesen Link für den "Kaufen"-Button.
    /// </summary>
    string? CheckoutUrl,
    float   Rating,
    int     Downloads,
    /// <summary>Neueste veröffentlichte Version (IsLatest + IsPublished).</summary>
    string? LatestVersion,
    /// <summary>Wann die neueste Version veröffentlicht wurde.</summary>
    DateTimeOffset? LatestPublishedAt,
    string? LatestTrustLevel,
    string? LatestRiskLevel,
    string? MinAaiaVersion,
    DateTimeOffset CreatedAt);

/// <summary>
/// Antwort auf GET /api/marketplace/extensions/{extensionId}/checkout.
/// Liefert alle für den Kaufprozess nötigen Informationen — ohne Auth.
///
/// WooCommerce-Produktseite und Module Manager nutzen diesen Endpunkt
/// um den "Kaufen"-Button und Preisanzeige zu befüllen.
/// </summary>
public sealed record ExtensionCheckoutDto(
    string   ExtensionId,
    string   DisplayName,
    string   LicenseModel,       // "Free" | "Paid" | "Subscription" | "Enterprise"
    /// <summary>Anzeigepreis, z.B. "€9.99" oder "€4.99/Monat". Null wenn Free.</summary>
    decimal? Price,
    string?  Currency,
    /// <summary>Direktlink zur LS/Paddle-Checkout-Seite. Null wenn Free oder nicht konfiguriert.</summary>
    string?  CheckoutUrl,
    /// <summary>MoR-Provider-Name: "LemonSqueezy" | "Paddle" | null.</summary>
    string?  MorProvider,
    bool     IsAvailable,        // true wenn CheckoutUrl gesetzt oder LicenseModel = Free
    string?  UnavailableReason); // z.B. "Checkout noch nicht konfiguriert."

/// <summary>
/// Request-Body für PUT /api/admin/extensions/{extensionId}/pricing.
/// Setzt Preis, Währung und Checkout-URL einer Extension.
/// </summary>
public sealed record SetExtensionPricingRequest(
    decimal? Price,
    string?  Currency,
    string?  CheckoutUrl,
    /// <summary>Optionaler Hinweis welcher MoR-Provider hinter CheckoutUrl steckt.</summary>
    string?  MorProvider = null);

/// <summary>
/// Vollständige Details zu einem einzelnen Release.
/// Rückgabe von GET /api/marketplace/extensions/{id}/releases/{version}.
/// </summary>
public sealed record RegistryReleaseDto(
    string  ExtensionId,
    string  Version,
    string  ExtensionType,
    bool    IsLatest,
    bool    IsPublished,
    bool    IsSignatureVerified,
    string? SignatureVersion,
    string? KeyFingerprint,
    string? SignedAtUtc,
    DateTimeOffset? SignatureVerifiedAt,
    DateTimeOffset? VisibleSinceAt,
    DateTimeOffset  PublishedAt,           // = Erstellungsdatum des Release-Eintrags
    string? PackageSha256,
    string? ManifestHash,
    string? TrustLevel,
    string? MinAaiaVersion,
    string? RiskLevel,
    /// <summary>Angefragte Berechtigungen als JSON-Array-String.</summary>
    string? Permissions,
    string? Changelog,
    string? DownloadUrl,
    string? SubmissionId,
    // Publisher-Info
    string  PublisherEtwId,
    string  PublisherDisplayName);

/// <summary>
/// Release-Kurzinfo für Listingzwecke.
/// Rückgabe von GET /api/marketplace/extensions/{id}/releases.
/// </summary>
public sealed record RegistryReleaseSummaryDto(
    string  Version,
    bool    IsLatest,
    bool    IsPublished,
    bool    IsSignatureVerified,
    string? TrustLevel,
    string? RiskLevel,
    DateTimeOffset? VisibleSinceAt,
    DateTimeOffset  PublishedAt);

/// <summary>Antwort auf GET /api/marketplace/extensions.</summary>
public sealed record RegistryExtensionListResponse(
    int                               TotalCount,
    int                               Page,
    int                               PageSize,
    IReadOnlyList<RegistryExtensionDto> Items);

/// <summary>Antwort auf GET /api/marketplace/extensions/{id}/releases.</summary>
public sealed record RegistryReleaseListResponse(
    string  ExtensionId,
    string  DisplayName,
    IReadOnlyList<RegistryReleaseSummaryDto> Releases);

/// <summary>Anfrage zum expliziten Veröffentlichen eines verifizierten Releases.</summary>
public sealed record PublishReleaseRequest(
    /// <summary>Optional: Für Phase 5.2 kann der Entwickler einen Changelog ergänzen.</summary>
    string? Changelog = null);

/// <summary>Antwort auf POST /api/marketplace/extensions/{id}/releases/{version}/publish.</summary>
public sealed record PublishReleaseResponse(
    bool    Success,
    string  ExtensionId,
    string  Version,
    string? Message,
    DateTimeOffset? VisibleSinceAt,
    string? Error);

// ── License-Status DTO (Phase 5.7b) ──────────────────────────────────────────

/// <summary>
/// Antwort auf GET /api/marketplace/extensions/{extensionId}/license-status.
/// Gibt den Lizenzstatus des authentifizierten Käufers für eine Extension zurück.
///
/// Authentifizierung: Bearer JWT mit email-Claim (Module-Manager-Login).
/// Logik: HasLicense = es existiert mindestens eine Lizenz, unabhängig von Status.
///        CanDownload = HasLicense AND Status = "Active" AND (ExpiresAt == null OR ExpiresAt > now).
/// </summary>
public sealed record ExtensionLicenseStatusDto(
    string  ExtensionId,
    string  LicenseModel,       // "Free" | "Paid" | "Subscription" | "Enterprise" | "Trial"
    bool    HasLicense,
    /// <summary>"Active" | "Expired" | "Revoked" | "Suspended" | null (wenn keine Lizenz)</summary>
    string? Status,
    DateTimeOffset? ExpiresAt,
    bool    CanDownload,
    /// <summary>Direktlink zur MoR-Checkout-Seite. Null wenn Free oder nicht konfiguriert.</summary>
    string? CheckoutUrl,
    /// <summary>Lizenz-Schlüssel (AAIA-ML-...) — nur zur Anzeige, nicht zur Authentifizierung.</summary>
    string? LicenseKey,
    /// <summary>Wann die Lizenz ausgestellt wurde.</summary>
    DateTimeOffset? IssuedAt);
