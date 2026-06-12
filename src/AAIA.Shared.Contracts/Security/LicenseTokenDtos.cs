namespace AAIA.Shared.Contracts.Security;

/// <summary>
/// Anfrage für ein neues Lizenz-Token.
/// Token wird an DeviceId gebunden — verhindert Weitergabe.
/// </summary>
public sealed record LicenseTokenRequest(
    string ModuleId,
    string BuyerEtwId,
    /// <summary>AAIAS-Geräte-ID (aus /api/server/info). Token gilt nur für dieses Gerät.</summary>
    string DeviceId);

/// <summary>
/// Signiertes RS256-JWT-Lizenz-Token.
/// AAIAS verifiziert es lokal mit dem Marketplace-Public-Key.
/// Keine API-Call bei jeder Ausführung nötig.
/// </summary>
public sealed record LicenseTokenDto(
    /// <summary>Signiertes JWT (RS256). Claims: sub, module_id, device_id, etw_id, exp, iat, jti.</summary>
    string         Token,
    string         ModuleId,
    string         BuyerEtwId,
    string         DeviceId,
    DateTimeOffset IssuedAt,
    DateTimeOffset ExpiresAt,
    /// <summary>Unique Token-ID — steht auf Revocation-Liste wenn Lizenz widerrufen wird.</summary>
    string         TokenId);

/// <summary>
/// Ergebnis der lokalen Token-Verifikation durch AAIAS.
/// Kein API-Call erforderlich — nur Public-Key-Verifikation + Revocation-Cache-Check.
/// </summary>
public sealed record LicenseTokenVerifyResult(
    bool    Valid,
    string  ModuleId,
    string? BuyerEtwId,
    string? DeviceId,
    DateTimeOffset? ExpiresAt,
    LicenseTokenFailReason FailReason = LicenseTokenFailReason.None,
    string? Detail = null);

public enum LicenseTokenFailReason
{
    None,
    InvalidSignature,
    Expired,
    DeviceMismatch,
    TokenRevoked,
    MalformedToken
}
