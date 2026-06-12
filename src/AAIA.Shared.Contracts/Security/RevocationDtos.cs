namespace AAIA.Shared.Contracts.Security;

/// <summary>Typ des gesperrten Objekts.</summary>
public enum RevocationTargetType
{
    /// <summary>Eine spezifische Modul-ID wird gesperrt.</summary>
    ModuleId,
    /// <summary>Alle Module eines ETW-Entwicklers werden gesperrt.</summary>
    EtwId,
    /// <summary>Ein Signing-Key wird widerrufen (alle damit signierten Module).</summary>
    KeyId
}

/// <summary>Grund für die Sperrung.</summary>
public enum RevocationReason
{
    SecurityVulnerability,
    MaliciousCode,
    PolicyViolation,
    DeveloperSuspended,
    KeyCompromised,
    LicenseViolation,
    RequestedByDeveloper,
    Other
}

/// <summary>Ein einzelner Revocation-Eintrag.</summary>
public sealed record RevocationEntryDto(
    string               Id,
    RevocationTargetType TargetType,
    string               TargetValue,     // ModuleId / ETW-ID / KeyId
    RevocationReason     Reason,
    string               Summary,
    DateTimeOffset       RevokedAt,
    DateTimeOffset?      EffectiveUntil); // null = dauerhaft

/// <summary>
/// Vollständige, signierte Revocation-Liste.
/// AAIAS cached diese lokal. Gültig für <see cref="ValidUntil"/>.
/// Signatur wird mit dem AAIA-Root-Key verifiziert.
/// </summary>
public sealed record RevocationListDto(
    IReadOnlyList<RevocationEntryDto> Entries,
    DateTimeOffset                    IssuedAt,
    DateTimeOffset                    ValidUntil,    // Cache-Ablauf (typisch: 24h)
    int                               SequenceNumber,
    string                            Signature);    // RS256 über IssuedAt+SequenceNumber+Entries-Hash

/// <summary>
/// Ergebnis einer Revocation-Prüfung für ein spezifisches Modul.
/// </summary>
public sealed record RevocationCheckResult(
    bool             IsRevoked,
    string           ModuleId,
    string?          EtwId,
    string?          KeyId,
    RevocationReason? Reason   = null,
    string?           Detail   = null,
    bool              FromCache = false);
