namespace AAIA.Shared.Contracts.Marketplace;

// ── Phase 5.8: Käuferkonto & Lizenz-Claim ────────────────────────────────────
//
// Trennung: Käufer-DTOs (User-Rolle) vs. Entwickler-DTOs (Developer/Partner).
// Buyer-JWT: Role = "User", Sub = BuyerAccount.Id.ToString()

// ── Requests ──────────────────────────────────────────────────────────────────

/// <summary>POST /api/account/register — Neues Käuferkonto anlegen.</summary>
public sealed record BuyerRegisterRequest(
    string Email,
    string Password,
    string? DisplayName = null);

/// <summary>POST /api/account/login — Käufer-Login.</summary>
public sealed record BuyerLoginRequest(
    string Email,
    string Password);

/// <summary>POST /api/account/licenses/claim — Claim-Token einlösen.</summary>
public sealed record BuyerClaimLicenseRequest(
    /// <summary>Der Klartext-Claim-Token aus dem E-Mail-Link (?token=...).</summary>
    string Token);

/// <summary>POST /api/account/licenses/resend-claim — Neue Claim-Mail anfordern.</summary>
public sealed record ResendClaimRequest(
    /// <summary>Der LicenseKey (z.B. "AAIA-ML-...") aus der Kaufbestätigung.</summary>
    string LicenseKey,
    /// <summary>E-Mail-Adresse die beim Kauf verwendet wurde.</summary>
    string BuyerEmail);

// ── Responses ─────────────────────────────────────────────────────────────────

/// <summary>
/// Antwort auf Register und Login.
/// Token ist ein JWT mit Role = "User" — wird an Buyer-Endpunkte als Bearer gesandt.
/// </summary>
public sealed record BuyerTokenResponse(
    string          Token,
    DateTimeOffset  ExpiresAt,
    int             BuyerAccountId,
    string          Email,
    string?         DisplayName);

/// <summary>
/// Lizenz-Detail aus Käufersicht.
/// Kein LicenseKey im Klartext — Käufer brauchen den Schlüssel nicht (kein Aktivierungsworkflow).
/// </summary>
public sealed record BuyerLicenseDto(
    int             LicenseId,
    string          ExtensionId,
    string          ExtensionName,
    string          LicenseModel,
    string          Status,
    DateTimeOffset  IssuedAt,
    DateTimeOffset? ExpiresAt,
    /// <summary>"Unclaimed" | "Claimed" | "Expired"</summary>
    string          ClaimStatus,
    DateTimeOffset? ClaimedAt);

/// <summary>Antwort auf POST /api/account/licenses/claim.</summary>
public sealed record BuyerClaimLicenseResponse(
    bool   Success,
    string Message,
    BuyerLicenseDto? License = null);
