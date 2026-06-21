using AAIA.Shared.Contracts.Extensions;

namespace AAIA.Shared.Contracts.Marketplace;

// ── Download DTOs (Phase 5.3) ─────────────────────────────────────────────────

/// <summary>
/// Metadaten zu einem herunterladbaren Release-Paket.
/// Optional — GET /download gibt direkt die Binärdaten zurück,
/// aber HEAD /download liefert diese Infos über Response-Header.
/// </summary>
public sealed record DownloadInfoDto(
    string  ExtensionId,
    string  Version,
    string  FileName,           // z.B. "aaia.fritzbox-1.0.0.aaiaext"
    long    FileSizeBytes,
    string  PackageSha256,      // Hex, lowercase
    string  ContentType,        // "application/octet-stream"
    string  TrustLevel,
    bool    IsSignatureVerified,
    string? KeyFingerprint,
    string? SignatureVersion);

/// <summary>
/// Ergebnis eines lokalen Downloads im Module Manager.
/// Wird nach abgeschlossenem HTTP-Download + Hash-Prüfung erzeugt.
/// </summary>
public sealed record LocalDownloadResult(
    bool    Success,
    string  ExtensionId,
    string  Version,
    string? LocalPath,          // absoluter Pfad zur gespeicherten .aaiaext-Datei
    string? ErrorMessage,
    bool    HashVerified,       // true = SHA256 stimmt überein
    long    FileSizeBytes,
    /// <summary>Gesetzter Lizenz-Fehlercode wenn Download abgelehnt wurde.</summary>
    DownloadDeniedReason DeniedReason = DownloadDeniedReason.None);

// ── Lizenz-Gate Fehlercodes (Phase 5.4a) ─────────────────────────────────────

/// <summary>
/// Grund für einen abgelehnten Download.
/// None = Download war erfolgreich oder Fehler war nicht lizenzbedingt.
/// </summary>
public enum DownloadDeniedReason
{
    None,
    AuthRequired,        // 401: Extension kostenpflichtig, kein Bearer-Token
    LicenseRequired,     // 403: Token OK, aber keine Lizenz → Kauf nötig
    SubscriptionExpired, // 403: Abo abgelaufen
    LicenseRevoked       // 403: Lizenz widerrufen / gesperrt
}

/// <summary>
/// Vom Server gesendeter Fehler-Body bei 401/403 auf /download.
/// Deserialisiert aus JSON-Response.
/// </summary>
public sealed record DownloadLicenseError(
    string            Error,                // "AuthRequired" | "LicenseRequired" | "SubscriptionExpired" | "LicenseRevoked"
    string?           LicenseModel = null,  // z.B. "Paid" (nur bei AuthRequired)
    string?           Detail       = null,
    AaiaLicenseModel  Model        = AaiaLicenseModel.Free)
{
    /// <summary>Übersetzt Error-String in DownloadDeniedReason.</summary>
    public DownloadDeniedReason ToReason() => Error switch
    {
        "AuthRequired"       => DownloadDeniedReason.AuthRequired,
        "LicenseRequired"    => DownloadDeniedReason.LicenseRequired,
        "SubscriptionExpired"=> DownloadDeniedReason.SubscriptionExpired,
        "LicenseRevoked"     => DownloadDeniedReason.LicenseRevoked,
        _                    => DownloadDeniedReason.None
    };
}
