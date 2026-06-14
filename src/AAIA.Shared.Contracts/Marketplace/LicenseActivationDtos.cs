namespace AAIA.Shared.Contracts.Marketplace;

/// <summary>
/// Lizenz-Aktivierungsanfrage vom AAIA Module Manager.
/// Kein JWT erforderlich — der LicenseKey ist das Credential.
/// Der Endpunkt ist rate-limitiert (5 req/min/IP).
/// </summary>
public sealed record LicenseActivationRequest(
    /// <summary>Lizenz-Schlüssel aus WooCommerce "Mein Konto" (Format: AAIA-ML-...).</summary>
    string LicenseKey,

    /// <summary>
    /// E-Mail-Adresse die beim Kauf angegeben wurde.
    /// Verhindert einfache Key-Weitergabe zwischen Nutzern.
    /// </summary>
    string BuyerEmail,

    /// <summary>
    /// Geräte-ID aus AAIAS (GET /api/server/info → DeviceId).
    /// JWT wird an diese ID gebunden — gilt nur auf diesem Gerät.
    /// Leer lassen für LanguagePacks (kein Device-Binding nötig).
    /// </summary>
    string DeviceId,

    /// <summary>
    /// Optionale Plausibilitätsprüfung: ModuleId aus dem Manifest.
    /// Wenn angegeben, wird geprüft ob der Key zu diesem Modul gehört.
    /// </summary>
    string? ModuleId = null);

/// <summary>
/// Antwort auf eine erfolgreiche Lizenz-Aktivierung.
/// Entweder LicenseJwt (Module/Plugin) oder DownloadUrl (LanguagePack) ist gesetzt.
/// </summary>
public sealed record LicenseActivationResponse(
    string LicenseKey,
    string ModuleId,
    string ExtensionType,   // "Module" | "Plugin" | "LanguagePack"

    /// <summary>
    /// Signiertes RS256-JWT für Module und Plugins.
    /// AAIAS verifiziert es lokal ohne API-Call.
    /// Null für LanguagePacks.
    /// </summary>
    string? LicenseJwt,

    /// <summary>
    /// Direkte Download-URL zum .aalang-Paket.
    /// Nur für LanguagePacks. Null für Module/Plugins.
    /// </summary>
    string? DownloadUrl,

    /// <summary>Zielsprache des Sprachpakets (z.B. "de-DE"). Null für Module/Plugins.</summary>
    string? Locale,

    DateTimeOffset IssuedAt,
    DateTimeOffset? ExpiresAt);
