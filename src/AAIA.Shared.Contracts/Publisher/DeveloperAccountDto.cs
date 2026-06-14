namespace AAIA.Shared.Contracts.Publisher;

// ── Enums ─────────────────────────────────────────────────────────────────────

public enum DeveloperRole
{
    /// <summary>Kostenlos. Kein Marketplace-Publish.</summary>
    Community,

    /// <summary>Identität geprüft. Marketplace-Zugang freigegeben.</summary>
    VerifiedDeveloper,

    /// <summary>Firma / Gewerbe. Kostenpflichtige Module erlaubt.</summary>
    Professional,

    /// <summary>Von AAIA offiziell freigegeben. Zugriff auf sensible APIs.</summary>
    TrustedDeveloper,

    /// <summary>Plattform-Eigentümer. Vollzugriff auf alle Admin-Funktionen.</summary>
    Owner
}

public enum DeveloperStatus
{
    Pending,
    Active,
    Suspended,
    Revoked
}

// ── Developer Account ─────────────────────────────────────────────────────────

/// <summary>
/// Entwickler-Account auf der AAIA Marketplace API.
/// Trägt eine dauerhafte, unveränderliche ETW-ID (z.B. "ETW-000001").
/// Lebt ausschließlich auf dem zentralen Webserver — NIEMALS in einer lokalen
/// AAIAS-Installation.
/// </summary>
public sealed record DeveloperAccountDto(
    string          EtwId,
    string          DisplayName,
    string          Email,
    string?         GitHubAccount,
    string?         NuGetProfile,
    DeveloperRole   Role,
    DeveloperStatus Status,
    bool            Verified,
    /// <summary>Fingerprint des registrierten Publisher-Zertifikats.</summary>
    string?         KeyId,
    /// <summary>Aggregierte Bewertung aus Modul-Ratings (0.0–5.0).</summary>
    float           Reputation,
    DateTimeOffset  CreatedAt,
    int             ModuleCount);

/// <summary>Öffentlich sichtbares Entwickler-Profil (für Marketplace-Seiten).</summary>
public sealed record DeveloperPublicProfileDto(
    string  EtwId,
    string  DisplayName,
    bool    Verified,
    float   Reputation,
    int     ModuleCount,
    string? IconUrl);

// ── Register / Login ──────────────────────────────────────────────────────────

/// <summary>Anfrage zur Registrierung eines neuen Entwickler-Accounts.</summary>
public sealed record DeveloperRegisterRequest(
    string  DisplayName,
    string  Email,
    string  Password,
    string? GitHubAccount = null,
    string? NuGetProfile  = null);

/// <summary>Antwort nach erfolgreicher Registrierung — enthält die vergebene ETW-ID.</summary>
public sealed record DeveloperRegisterResponse(
    string        EtwId,
    string        DisplayName,
    DeveloperRole Role,
    string        Message,
    /// <summary>otpauth://totp/... URI für QR-Code-Scan. Nur einmalig bei Registrierung zurückgegeben.</summary>
    string?       TotpUri    = null,
    /// <summary>TOTP-Secret im Klartext für manuelle Eingabe in Authenticator-App.</summary>
    string?       TotpSecret = null);

/// <summary>Login-Anfrage gegen die Marketplace API.</summary>
public sealed record DeveloperLoginRequest(
    string  Email,
    string  Password,
    /// <summary>6-stelliger TOTP-Code aus der Authenticator-App. Pflichtfeld sobald 2FA aktiviert ist.</summary>
    string? TotpCode = null);

/// <summary>Stabile Auth-Fehlercodes für Clients. Meldungstexte dürfen lokalisiert werden.</summary>
public static class DeveloperAuthErrorCodes
{
    public const string InvalidCredentials = "INVALID_CREDENTIALS";
    public const string TotpRequired       = "TOTP_REQUIRED";
    public const string TotpInvalid        = "TOTP_INVALID";
    public const string AccountSuspended   = "ACCOUNT_SUSPENDED";
    public const string AccountRevoked     = "ACCOUNT_REVOKED";
}

/// <summary>Login-Antwort mit JWT für nachfolgende API-Aufrufe.</summary>
public sealed record DeveloperLoginResponse(
    string         EtwId,
    string         DisplayName,
    string         AccessToken,
    DateTimeOffset ExpiresAt,
    DeveloperRole  Role = DeveloperRole.Community);

// ── Publisher Certificate ─────────────────────────────────────────────────────

/// <summary>
/// Registriert den öffentlichen Schlüssel eines Entwicklers.
/// Der private Schlüssel bleibt lokal beim Entwickler (aaia-sign keygen).
/// </summary>
public sealed record RegisterPublisherKeyRequest(
    string PublicKeyPem,
    string KeyId,
    string Algorithm = "RSA-PSS-SHA256");

/// <summary>Antwort nach Key-Registrierung.</summary>
public sealed record RegisterPublisherKeyResponse(
    string         KeyId,
    string         EtwId,
    DateTimeOffset RegisteredAt);
