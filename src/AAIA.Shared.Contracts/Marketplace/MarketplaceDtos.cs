using AAIA.Shared.Contracts.Extensions;

namespace AAIA.Shared.Contracts.Marketplace;

/// <summary>
/// Einzelner Eintrag im Marketplace-Feed.
/// v2.1: ETW-ID, Preis, Rating, LicenseModel ergänzt — alle neuen Felder optional
/// für Rückwärtskompatibilität mit bestehenden Feeds.
/// </summary>
public sealed record MarketplaceModuleDto(
    string   Id,
    string   DisplayName,
    string   Version,
    string   Description,
    string   Category,
    string   PluginClass,        // "ServerModule" | "SystemPlugin" | "ClientPlugin"
    bool     Free,
    string?  PriceHint,          // null wenn Free, sonst z.B. "€4,99/Monat" (dekorativ)
    string[] Platforms,
    string?  DownloadUrl,        // URL zum Paket (vom Webserver)
    string?  ManifestUrl,        // URL zur aaia-extension.json
    string?  ChangelogUrl,
    string   PublisherId,
    string   MinServerVersion,
    string?  IconUrl,
    string[] Tags,

    // ── v2.1: Publisher / Preis / Rating ─────────────────────────────────────
    string?          PublisherEtwId          = null,
    string?          PublisherDisplayName    = null,
    bool             PublisherVerified       = false,
    AaiaLicenseModel LicenseModel            = AaiaLicenseModel.Free,
    decimal?         Price                   = null,
    string?          Currency                = null,
    float            Rating                  = 0f,
    int              Downloads               = 0,
    string[]?        Screenshots             = null,
    string?          LatestVersion           = null);

/// <summary>Der komplette Feed — gecacht von MarketplaceFeedService / AAIAS-Proxy.</summary>
public sealed record MarketplaceFeedDto(
    string                              Version,
    DateTimeOffset                      GeneratedAt,
    IReadOnlyList<MarketplaceModuleDto> Modules);

/// <summary>Anfrage für Marketplace-Install aus Feed.</summary>
public sealed record MarketplaceInstallRequest(
    string ModuleId,
    bool   OverwriteExisting = false);

/// <summary>Ergebnis eines Marketplace-Installs.</summary>
public sealed record MarketplaceInstallResult(
    bool    Success,
    string  ModuleId,
    string? Error,
    string? InstalledVersion);

/// <summary>
/// Lizenz-Zuweisung: Ob ein Benutzer (identifiziert per E-Mail) ein Modul
/// freigeschaltet hat. Abfrage gegen die Marketplace API.
/// </summary>
public sealed record LicenseCheckResult(
    bool             HasLicense,
    string           ModuleId,
    string           BuyerEmail,
    AaiaLicenseModel LicenseModel,
    DateTimeOffset?  ExpiresAt,
    /// <summary>Stripe / PayPal Transaction-ID — null wenn manuell zugewiesen.</summary>
    string?          TransactionId);

/// <summary>Veröffentlichungsanfrage (Marketplace API POST /api/modules/{id}/publish).</summary>
public sealed record ModulePublishRequest(
    string  ModuleId,
    string  Version,
    string  Changelog,
    string? NuGetVersion   = null,
    string? GitHubRelease  = null);

/// <summary>Stripe Checkout-Session erstellen.</summary>
public sealed record CreateCheckoutSessionRequest(
    string  ModuleId,
    string  BuyerEmail,
    string? BuyerEtwId,
    string  SuccessUrl,
    string  CancelUrl);

/// <summary>Antwort nach Erstellen einer Stripe Checkout-Session.</summary>
public sealed record CheckoutSessionDto(
    string SessionId,
    string CheckoutUrl);

// ── WooCommerce Bridge ────────────────────────────────────────────────────────

/// <summary>
/// POST /api/marketplace/orders/woocommerce/confirm
/// WooCommerce-Plugin sendet das nach Abschluss einer WC-Bestellung.
/// Auth: X-Bridge-Key Header (serverseitiger API-Schlüssel).
/// </summary>
public sealed record WooOrderConfirmRequest(
    int     WcOrderId,
    /// <summary>_aaia_product_code aus WooCommerce-Produkt-Meta.</summary>
    string  ProductCode,
    string  BuyerEmail,
    int?    BuyerWpUserId = null,
    /// <summary>Optional — wenn vorhanden wird ein JWT-Token ausgestellt.</summary>
    string? DeviceId = null);

/// <summary>Antwort nach Lizenz-Bestätigung durch die Marketplace-API.</summary>
public sealed record WooOrderConfirmResponse(
    string          LicenseKey,
    string?         LicenseJwt,
    string          ModuleId,
    DateTimeOffset  IssuedAt,
    DateTimeOffset? ExpiresAt,
    /// <summary>true = neue Lizenz angelegt; false = bestehende Lizenz zurückgegeben (idempotent).</summary>
    bool IsNew);

/// <summary>
/// POST /api/marketplace/orders/woocommerce/verify
/// WooCommerce-Plugin prüft ob ein Lizenz-Schlüssel noch gültig ist.
/// Auth: X-Bridge-Key Header.
/// </summary>
public sealed record WooVerifyLicenseRequest(string LicenseKey);

/// <summary>Antwort auf eine Lizenz-Verifikationsanfrage vom WooCommerce-Plugin.</summary>
public sealed record WooVerifyLicenseResponse(
    bool            Valid,
    string?         Reason,
    string?         ModuleId,
    string?         ProductCode,
    string?         Status,
    DateTimeOffset? ExpiresAt);

/// <summary>Antwort nach erfolgreicher Veröffentlichung.</summary>
public sealed record ModulePublishResponse(
    bool    Success,
    string  ModuleId,
    string  Version,
    string? DownloadUrl,
    string? MarketplaceUrl,
    string? Error);
