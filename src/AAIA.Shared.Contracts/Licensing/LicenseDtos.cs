namespace AAIA.Shared.Contracts.Licensing;

// ── Lizenz-Status ─────────────────────────────────────────────────────────────

public enum LicenseStatus
{
    Active,
    Expired,
    Revoked,
    Invalid
}

// ── DTOs ──────────────────────────────────────────────────────────────────────

/// <summary>
/// Einzelne Lizenz wie sie der API-Aufrufer sieht.
/// Enthält nie den raw Key-Hash — nur display-sichere Felder.
/// </summary>
public sealed record ModuleLicenseDto(
    string        Id,
    string        ModuleId,
    string        ModuleName,
    string        KeyPreview,       // z.B. "AAIA-3F9A****-....-****"
    DateTimeOffset ActivatedAt,
    DateTimeOffset? ExpiresAt,       // null = perpetual
    string        ActivatedBy,
    LicenseStatus Status);

/// <summary>Anfrage zum Aktivieren eines Lizenzschlüssels.</summary>
public sealed record ActivateLicenseRequest(
    string ModuleId,
    string LicenseKey);

/// <summary>Antwort auf eine Aktivierungsanfrage.</summary>
public sealed record ActivateLicenseResponse(
    bool   Success,
    string LicenseId,
    string? Error,
    DateTimeOffset? ExpiresAt);

/// <summary>Anfrage zum Generieren eines neuen Lizenzschlüssels (Admin only).</summary>
public sealed record GenerateLicenseRequest(
    string  ModuleId,
    int?    ValidDays);     // null = perpetual

/// <summary>Generierter Schlüssel — nur einmalig anzeigbar.</summary>
public sealed record GenerateLicenseResponse(
    string LicenseKey,
    string ModuleId,
    DateTimeOffset? ExpiresAt);

/// <summary>Kompaktes Validierungsergebnis — für Module zur internen Prüfung.</summary>
public sealed record LicenseCheckResult(
    bool           IsValid,
    LicenseStatus  Status,
    DateTimeOffset? ExpiresAt,
    string?        Reason);
