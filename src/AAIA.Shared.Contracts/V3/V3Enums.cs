namespace AAIA.Shared.Contracts.V3;

// ─────────────────────────────────────────────────────────────────────────────
// AAIA V3 — Enums
// Zentrale Aufzählungstypen für die V3-Plattform.
// Bestehende V2-Enums bleiben unverändert; diese Typen sind V3-spezifisch.
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Benutzerrollen innerhalb von AAIA.</summary>
public enum AaiaUserRole
{
    ReadOnly,
    ReadWrite,
    Admin,
    SuperAdmin
}

/// <summary>Lebenszyklusstatus eines Work Orders.</summary>
public enum WorkOrderStatus
{
    Open,
    InProgress,
    Completed,
    Failed,
    Cancelled,
    Archived
}

/// <summary>Prioritätsstufe eines Work Orders.</summary>
public enum WorkOrderPriority
{
    Low,
    Normal,
    High,
    Critical
}

/// <summary>Kategorie eines Memory-Eintrags im AAIAM-System.</summary>
public enum MemoryCategory
{
    Projects,
    Documents,
    Conversations,
    Templates,
    Archive,
    System
}

/// <summary>Status eines Inbox-Events.</summary>
public enum InboxEventStatus
{
    New,
    Read,
    Archived,
    Deleted
}

/// <summary>Anzeigefarbe eines Provider-Status-Chips in der TopBar.</summary>
public enum ProviderStatusColor
{
    Green,
    Yellow,
    Red,
    Grey
}

/// <summary>
/// DUKI-Berechtigungsumfang (Delegation, User-Kontrolle, KI-Autonomie).
/// Bestimmt, welche Aktionen ein KI-Agent ohne explizite Freigabe ausführen darf.
/// </summary>
public enum DukiScope
{
    /// <summary>Nur offene (lesbare) Aktionen erlaubt.</summary>
    OpenOnly,
    /// <summary>Nur Lesezugriff; keine schreibenden Aktionen.</summary>
    ReadOnly,
    /// <summary>Agent kann Aktionen vorschlagen; Benutzer führt sie aus.</summary>
    AssistedSession,
    /// <summary>Agent darf Aktionen ausführen, die laufend überwacht werden.</summary>
    SupervisedExec,
    /// <summary>Agent darf delegierte Aktionen ohne Einzelfreigabe ausführen.</summary>
    DelegatedExec,
    /// <summary>Vollständige Autonomie — nur in explizit freigegebenen Szenarien.</summary>
    Autonomous
}

/// <summary>Installationsstatus eines Moduls.</summary>
public enum ModuleStatus
{
    Installed,
    Active,
    Inactive,
    Error,
    Updating,
    Deprecated
}

/// <summary>Vertrauensstufe einer Extension aus dem Marketplace.</summary>
public enum TrustLevel
{
    Official,
    Verified,
    Community,
    Unverified
}

/// <summary>Risikostufe einer Extension oder DUKI-Aktion.</summary>
public enum RiskLevel
{
    Low,
    Medium,
    High,
    Critical
}

// ─────────────────────────────────────────────────────────────────────────────
// V3.1c — Approval + Audit Enums
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Status einer Freigabe-Anforderung im Gatekeeper.</summary>
public enum ApprovalStatus
{
    Pending,
    Approved,
    Denied,
    Expired,
    Cancelled
}

/// <summary>Typ einer Freigabe-Anforderung.</summary>
public enum ApprovalType
{
    ExtensionEnable,
    ExtensionDisable,
    DukiAction,
    PermissionChange,
    RiskyOperation
}

/// <summary>
/// Typ einer Extension.
/// Language Packs und Themes enthalten keinen ausführbaren Code.
/// Module sind First-Party, Plugins Third-Party.
/// </summary>
public enum ExtensionType
{
    /// <summary>First-Party Servermodul (ausführbarer Code, AAIAS-seitig).</summary>
    Module,
    /// <summary>Third-Party Erweiterung (ausführbarer Code, Client- oder Serverseitig).</summary>
    Plugin,
    /// <summary>Verbindet externe Dienste oder MCP-Endpunkte (ausführbarer Code).</summary>
    Connector,
    /// <summary>Visuelles Theme — enthält keinen ausführbaren Code.</summary>
    Theme,
    /// <summary>Sprachpaket — enthält keinen ausführbaren Code.</summary>
    LanguagePack
}

/// <summary>
/// Auth-Modus fuer den V3 RoleGuard.
/// Steuert wie ICurrentUserContext pro Request befuellt wird.
/// </summary>
public enum V3AuthMode
{
    /// <summary>
    /// DevelopmentHeader: X-Aaia-Role Header.
    /// NUR aktiv wenn AAIA_DEV_MODE=1 gesetzt ist.
    /// Nicht fuer Produktion.
    /// </summary>
    DevelopmentHeader,

    /// <summary>
    /// LocalSession: V2 Bearer-Token via ServerAuthService.
    /// Standard-Modus fuer lokale AAIAS-Instanzen.
    /// Authorization: Bearer [token] aus POST /api/auth/login.
    /// </summary>
    LocalSession,

    /// <summary>
    /// Jwt: Externer JWT-Token. V3.4 Foundation-Stub.
    /// Noch nicht implementiert.
    /// </summary>
    Jwt,
}

// ─────────────────────────────────────────────────────────────────────────────
// V3.2 Security Pairing — Enums
// Gerätebindung, Pairing-Sessions, Remote-Access-Richtlinie.
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>Formfaktor des zu verbindenden Geräts.</summary>
public enum DeviceType
{
    Desktop,
    Mobile,
    Browser,
    Service
}

/// <summary>Betriebssystem-Plattform des Geräts.</summary>
public enum DevicePlatform
{
    Windows,
    MacOS,
    Linux,
    iOS,
    Android,
    Web
}

/// <summary>
/// Vertrauensstatus eines registrierten Geräts.
/// Hinweis: Nicht zu verwechseln mit TrustLevel (Extension-Marketplace).
/// </summary>
public enum DeviceTrustStatus
{
    /// <summary>Gerät wurde registriert, aber noch nicht von einem Admin freigegeben.</summary>
    Pending,
    /// <summary>Gerät ist vertrauenswürdig — Admin hat explizit freigegeben.</summary>
    Trusted,
    /// <summary>Vertrauen wurde aktiv entzogen.</summary>
    Revoked,
    /// <summary>Gerät wurde durch Ablauf der TTL automatisch deaktiviert.</summary>
    Expired
}
