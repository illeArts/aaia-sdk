namespace AAIA.Shared.Contracts.V3;

// ─────────────────────────────────────────────────────────────────────────────
// AAIA V3 — Data Transfer Objects
// Alle V3-spezifischen DTOs für API-Responses und interne Kommunikation.
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Systemgesundheitsstatus — Antwort von GET /api/v3/system/health</summary>
public sealed record HealthStatusDto(
    string Status,
    string Version,
    string ApiVersion,
    DateTimeOffset Timestamp,
    IReadOnlyList<HealthComponentDto> Components
);

/// <summary>Einzelne Komponente im Health-Check.</summary>
public sealed record HealthComponentDto(
    string Name,
    string Status,
    string? Detail = null
);

/// <summary>Status eines KI-Providers — Antwort von GET /api/v3/providers</summary>
public sealed record ProviderStatusDto(
    string Id,
    string Name,
    string Type,
    bool IsConnected,
    ProviderStatusColor StatusColor,
    string? StatusMessage = null,
    string? ModelId = null
);

/// <summary>Manifest eines installierten Moduls.</summary>
public sealed record ModuleManifestDto(
    string Id,
    string Name,
    string Version,
    string Description,
    string Author,
    ModuleStatus Status,
    TrustLevel Trust,
    IReadOnlyList<string> Capabilities,
    bool IsFirstParty
);

/// <summary>Vollständiges Extension-Manifest V2.1 (maschinenlesbar).</summary>
public sealed record ExtensionManifestDto(
    string Id,
    string Name,
    string Version,
    ExtensionType Type,
    string Category,
    string Description,
    string Author,
    IReadOnlyList<string> Permissions,
    IReadOnlyList<string> RequiredCapabilities,
    IReadOnlyList<string> OptionalCapabilities,
    IReadOnlyList<string> SupportedPlatforms,
    ExtensionUiDto? Ui,
    ExtensionBackgroundDto? Background,
    ExtensionResourcesDto? Resources,
    IReadOnlyList<string> Features,
    ExtensionLicensingDto Licensing,
    ExtensionMigrationDto? Migration
);

/// <summary>UI-Deklaration einer Extension.</summary>
public sealed record ExtensionUiDto(
    string? EntryPoint,
    string? Icon,
    IReadOnlyList<string>? Routes
);

/// <summary>Background-Service-Deklaration einer Extension.</summary>
public sealed record ExtensionBackgroundDto(
    string? EntryPoint,
    bool RunsOnServer
);

/// <summary>Ressourcen einer Extension (Bilder, Fonts, etc.).</summary>
public sealed record ExtensionResourcesDto(
    IReadOnlyList<string> Files,
    long MaxBundleSizeKb
);

/// <summary>Lizenzdeklaration einer Extension.</summary>
public sealed record ExtensionLicensingDto(
    string Model,
    bool RequiresActivation,
    string? LicenseUrl
);

/// <summary>Migrationsinformation bei Extension-Updates.</summary>
public sealed record ExtensionMigrationDto(
    string FromVersion,
    string ToVersion,
    IReadOnlyList<string> BreakingChanges
);

/// <summary>Sprachpaket-Manifest (kein ausführbarer Code).</summary>
public sealed record LanguagePackManifestDto(
    string Id,
    string Name,
    string Version,
    string LocaleCode,
    string LocaleName,
    string Author,
    double CoveragePercent,
    IReadOnlyList<string> SupportedPlatforms
);

/// <summary>Installierte Extension mit Status.</summary>
public sealed record InstalledExtensionDto(
    string Id,
    string Name,
    string Version,
    ExtensionType Type,
    ModuleStatus Status,
    TrustLevel Trust,
    RiskLevel Risk,
    DateTimeOffset InstalledAt,
    bool IsEnabled
);

/// <summary>Navigationseintrag für die WebUI-SideNav.</summary>
public sealed record NavigationRouteDto(
    string Id,
    string Label,
    string Route,
    string Icon,
    AaiaUserRole MinRole,
    bool IsActive,
    IReadOnlyList<NavigationRouteDto>? Children = null
);

/// <summary>Standardisierter API-Fehler — konsistent über alle V3-Endpunkte.</summary>
public sealed record ApiErrorDto(
    string Code,
    string Message,
    string? Detail = null,
    string? TraceId = null
);

// ─────────────────────────────────────────────────────────────────────────────
// V3.1c — Approval + Audit DTOs
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Freigabe-Anforderung für riskante Aktionen (Extension-Aktivierung, DUKI-Aktionen, etc.).
/// Wird im Gatekeeper / Approval Center angezeigt.
/// </summary>
public sealed record ApprovalRequestDto(
    string Id,
    ApprovalType Type,
    string SourceId,
    string SourceName,
    string Title,
    string Description,
    RiskLevel RiskLevel,
    AaiaUserRole RequiredRole,
    ApprovalStatus Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ExpiresAt = null,
    IReadOnlyList<string>? Permissions = null,
    IReadOnlyList<string>? Capabilities = null
);

/// <summary>Audit-Eintrag — protokolliert sicherheitsrelevante Aktionen.</summary>
public sealed record AuditEntryDto(
    string Id,
    DateTimeOffset Timestamp,
    string Actor,
    string Action,
    string TargetId,
    string TargetType,
    RiskLevel RiskLevel,
    string Result,       // "success" | "failure" | "denied"
    string? Message = null
);

// ─────────────────────────────────────────────────────────────────────────────
// V3.2 — AIRI Settings DTOs
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// AIRI Companion Einstellungen — Antwort von GET /api/v3/airi/settings.
/// Sicherheits-Invariante: duki.autonomous ist nie konfigurierbar.
/// Kamera/Mikrofon sind optionale Hardware-Capabilities, nicht hier steuerbar.
/// TTS ist ein Stub-Flag ohne Hardware-Implementierung in V3.
/// </summary>
public sealed record AiriSettingsDto(
    string  Mode,                  // "off"|"minimal"|"helpful"|"lively"|"playful"|"focus"
    bool    AnimationsEnabled,
    bool    TtsEnabled,            // Text-to-Speech Stub — keine Hardware-Impl. in V3
    bool    IdleReactionsEnabled,
    bool    WarningsEnabled,
    int     HumorLevel,            // 0–100
    bool    ShowContextPanel,
    bool    ShowFloating,
    bool    ShowTopBar,
    bool    ReactionToWorkorders,
    bool    ReactionToProvider,
    bool    ReactionToDuki,
    bool    ReactionToSecurity,
    bool    DukiRunPermission      // duki.run erlaubt — NICHT duki.autonomous
);

/// <summary>Patch-Request für AIRI Settings — nur gesetzte Felder werden überschrieben.</summary>
public sealed record AiriSettingsPatchDto(
    string? Mode                 = null,
    bool?   AnimationsEnabled    = null,
    bool?   TtsEnabled           = null,
    bool?   IdleReactionsEnabled = null,
    bool?   WarningsEnabled      = null,
    int?    HumorLevel           = null,
    bool?   ShowContextPanel     = null,
    bool?   ShowFloating         = null,
    bool?   ShowTopBar           = null,
    bool?   ReactionToWorkorders = null,
    bool?   ReactionToProvider   = null,
    bool?   ReactionToDuki       = null,
    bool?   ReactionToSecurity   = null,
    bool?   DukiRunPermission    = null
);

/// <summary>Systemkonfiguration — Antwort von GET /api/v3/config</summary>
public sealed record SystemConfigDto(
    string ServerVersion,
    string ApiVersion,
    string InstanceId,
    bool DeveloperMode,
    bool TlsEnabled,
    string DefaultLocale,
    IReadOnlyDictionary<string, object> Features
);

// ─────────────────────────────────────────────────────────────────────────────
// V3.3 — Auth Me DTO
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Antwort von GET /api/v3/auth/me.
/// Gibt den aktuellen Nutzerkontext zurück wie er vom V3RoleGuard gesetzt wurde.
/// IsAuthenticated=false → kein gültiger Token vorhanden.
/// IsDevelopmentMode=true → DevMode aktiv (X-Aaia-Role Header erlaubt).
/// </summary>
public sealed record AuthMeDto(
    string UserId,
    string DisplayName,
    string Role,              // AaiaUserRole als String (ReadOnly|ReadWrite|Admin|SuperAdmin)
    string AuthMode,          // V3AuthMode als String (LocalSession|DevelopmentHeader|Jwt)
    bool   IsAuthenticated,
    bool   IsDevelopmentMode
);

// ─────────────────────────────────────────────────────────────────────────────
// V3.2 Security Pairing Foundation — DTOs
// Pairing-Sessions, TrustedDevices, RemoteAccess-Policy, VPN-Status.
// Sicherheits-Invariante: Kein Secret, kein Private Key, kein OTP im QR-Code.
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Aktive Pairing-Session — Antwort von POST /api/v3/security/pairing.
/// Enthält NUR öffentliche Felder; kein Secret, kein Private Key.
/// TTL = 2 Minuten. Einmalig verwendbar (IsUsed=true nach Abschluss).
/// </summary>
public sealed record PairingSessionDto(
    string PairingSessionId,
    string ServerId,
    string Challenge,
    string Nonce,
    string IntendedClientType,
    string[] RequestedScopes,
    DateTimeOffset ExpiresAt,
    bool IsUsed,
    string CreatedByUserId,
    DateTimeOffset CreatedAtUtc
);

/// <summary>Request für POST /api/v3/security/pairing — startet eine neue Pairing-Session.</summary>
public sealed record CreatePairingSessionRequest(
    string IntendedClientType,
    string[] RequestedScopes
);

/// <summary>
/// Request für POST /api/v3/security/pairing/{id}/complete.
/// Schließt eine Pairing-Session ab und registriert das Gerät als TrustedDevice (Status=Pending).
/// </summary>
public sealed record CompletePairingRequest(
    string PairingSessionId,
    string ChallengeResponse,
    string DeviceDisplayName,
    string DeviceType,
    string Platform,
    string PublicKeyFingerprint
);

/// <summary>
/// Registriertes Gerät — Antwort von GET /api/v3/security/devices/{id}.
/// TrustLevel ist DeviceTrustStatus als String (Pending|Trusted|Revoked|Expired).
/// Kein Private Key, kein Secret in der Antwort.
/// </summary>
public sealed record TrustedDeviceDto(
    string DeviceId,
    string DisplayName,
    string DeviceType,
    string Platform,
    string PublicKeyFingerprint,
    string TrustLevel,              // DeviceTrustStatus als String
    string CreatedByUserId,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? LastSeenAtUtc,
    DateTimeOffset? RevokedAtUtc
);

/// <summary>Request für PATCH /api/v3/security/devices/{id}/trust — erteilt Gerät Vertrauen.</summary>
public sealed record TrustDeviceRequest(string DeviceId);

/// <summary>Request für POST /api/v3/security/devices/{id}/revoke — entzieht Gerät Vertrauen.</summary>
public sealed record RevokeDeviceRequest(string DeviceId, string? Reason = null);

/// <summary>
/// Remote-Access-Policy — Antwort von GET /api/v3/security/remote-access.
/// Alle Flags sind standardmäßig FALSE (deny-by-default).
/// AllowPublicInternet ist in V3.2 nicht konfigurierbar und bleibt immer false.
/// </summary>
public sealed record RemoteAccessPolicyDto(
    bool Enabled,
    bool AllowLan,
    bool AllowVpn,
    bool AllowPublicInternet,       // V3.2: immer false, nicht konfigurierbar
    bool RequireTrustedDevice,
    bool RequireTotpForAdmin,
    bool RequireTotpForDukiS3Plus,
    bool AuditAllRemoteRequests
);

/// <summary>Request für PATCH /api/v3/security/remote-access — aktualisiert die Policy.</summary>
public sealed record UpdateRemoteAccessPolicyRequest(
    bool? Enabled                  = null,
    bool? AllowLan                 = null,
    bool? AllowVpn                 = null,
    bool? RequireTrustedDevice     = null,
    bool? RequireTotpForAdmin      = null,
    bool? RequireTotpForDukiS3Plus = null,
    bool? AuditAllRemoteRequests   = null
    // AllowPublicInternet: absichtlich nicht konfigurierbar in V3.2
);

/// <summary>
/// VPN-Statusanzeige — Antwort von GET /api/v3/security/vpn/status.
/// V3.2: Nur Dokumentation und Status-DTOs. Kein eigener VPN-Server.
/// </summary>
public sealed record VpnStatusDto(
    bool IsConnected,
    string? ConnectedProfile,
    string Notes                    // Hinweis: "V3.2 — VPN Foundation (Doku/Status only, kein eigener Server)"
);

/// <summary>TOTP-Status-Stub — Antwort von GET /api/v3/security/totp/status.</summary>
public sealed record TotpStatusDto(
    bool IsEnabled,
    string? LastVerifiedAtUtc,
    string Note                     // "TOTP-Implementierung: V3.3+ Foundation Stub"
);
