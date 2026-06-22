namespace AAIA.Shared.Contracts.Marketplace;

// ── MoR Gate DTOs (Phase 5.12) ────────────────────────────────────────────────

// ── Enums ─────────────────────────────────────────────────────────────────────

/// <summary>
/// Verbindungsstatus eines ETW-MoR-Kontos oder Extension-Produkts.
/// Aufsteigend: NotConnected → Active.
/// </summary>
public enum MorConnectionStatus
{
    NotConnected       = 0,
    AccountLinked      = 1,
    ProductLinked      = 2,
    WebhookVerified    = 3,
    CommissionVerified = 4,
    Active             = 5,
    Suspended          = 6,
    Rejected           = 7
}

/// <summary>Wie AAIA die Provision erhält.</summary>
public enum CommissionMode
{
    Unknown   = 0,
    Affiliate = 1,
    Split     = 2,
    Manual    = 3,
    None      = 4
}

// ── DeveloperMorAccount DTOs ──────────────────────────────────────────────────

public sealed record DeveloperMorAccountDto(
    int                Id,
    string             DeveloperEtwId,
    string             Provider,
    string?            ExternalVendorId,
    string?            AffiliateId,
    string?            PartnerId,
    string?            ReferralCode,
    MorConnectionStatus Status,
    decimal            CommissionPercentForAaia,
    DateTimeOffset     ConnectedAt,
    DateTimeOffset?    VerifiedAt,
    DateTimeOffset?    SuspendedAt,
    DateTimeOffset     CreatedAt,
    DateTimeOffset     UpdatedAt);

public sealed record CreateDeveloperMorAccountRequest(
    string  Provider,
    string? ExternalVendorId,
    string? AffiliateId,
    string? PartnerId,
    string? ReferralCode);

// ── ExtensionMorProduct DTOs ──────────────────────────────────────────────────

public sealed record ExtensionMorProductDto(
    int                Id,
    string             ExtensionId,
    string             DeveloperEtwId,
    string             Provider,
    string?            ExternalProductId,
    string?            ExternalVariantId,
    string?            CheckoutUrl,
    string             LicenseModel,
    decimal            AaiaCommissionPercent,
    CommissionMode     CommissionMode,
    MorConnectionStatus Status,
    DateTimeOffset?    WebhookVerifiedAt,
    DateTimeOffset?    CommissionVerifiedAt,
    DateTimeOffset     CreatedAt,
    DateTimeOffset     UpdatedAt);

public sealed record CreateExtensionMorProductRequest(
    string  Provider,
    string? ExternalProductId,
    string? ExternalVariantId,
    string? CheckoutUrl,
    string  LicenseModel,
    decimal AaiaCommissionPercent,
    CommissionMode CommissionMode);

// ── PublishGate DTOs ──────────────────────────────────────────────────────────

/// <summary>
/// Ergebnis der Publish-Gate-Prüfung für eine Extension.
/// Alle Felder sind boolesche Bedingungen — bei CanPublish=false sind Blockers befüllt.
/// </summary>
public sealed record PublishGateResultDto(
    bool              CanPublish,
    string            ExtensionId,
    string            DeveloperEtwId,
    bool              IsSignatureVerified,
    bool              HasMorProduct,
    bool              IsMorProductActive,
    bool              IsCommissionConfigured,
    bool              IsWebhookVerified,
    bool              IsNotBlocked,
    string[]          Blockers,
    DateTimeOffset    CheckedAt);

// ── Admin-Requests ────────────────────────────────────────────────────────────

public sealed record AdminVerifyMorAccountRequest(
    decimal CommissionPercentForAaia,
    string? Notes);

public sealed record AdminSuspendMorAccountRequest(string Reason);

public sealed record AdminVerifyCommissionRequest(
    decimal AaiaCommissionPercent,
    CommissionMode CommissionMode);

public sealed record AdminSuspendMorProductRequest(string Reason);
