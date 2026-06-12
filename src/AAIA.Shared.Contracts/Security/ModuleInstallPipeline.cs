namespace AAIA.Shared.Contracts.Security;

// ── Installationspipeline-Contracts ───────────────────────────────────────────
//
// Diese Contracts definieren die Schnittstelle der Installationspipeline.
// Die eigentliche Implementierung liegt in AAIAS (ModuleInstallPipeline.cs).
// Die Contracts leben hier damit Module-Manager, Tests und Tools
// gegen die Pipeline arbeiten können ohne AAIAS zu referenzieren.
//
// Pipeline-Schritte (sequenziell, jeder Fehler stoppt die Pipeline):
//
//   Received → Extracted → HashVerified → ManifestVerified →
//   SignatureVerified → EtwVerified → RevocationChecked →
//   InspectorPassed → RightsApproved → LicenseVerified →
//   BackupCreated → SandboxInstalled → RegistryEntryCreated →
//   AuditWritten → Active
//
// Rollback ist von jedem Schritt ab SandboxInstalled möglich.

/// <summary>Einzelner Schritt der Installationspipeline.</summary>
public enum InstallPipelineStep
{
    Received            = 0,
    Extracted           = 1,
    HashVerified        = 2,
    ManifestVerified    = 3,
    SignatureVerified   = 4,
    EtwVerified         = 5,
    RevocationChecked   = 6,
    InspectorPassed     = 7,
    RightsApproved      = 8,
    LicenseVerified     = 9,
    BackupCreated       = 10,
    SandboxInstalled    = 11,
    RegistryEntryCreated = 12,
    AuditWritten        = 13,
    Active              = 14
}

/// <summary>Warum ein Schritt fehlgeschlagen ist.</summary>
public enum InstallFailReason
{
    None,
    HashMismatch,
    ManifestInvalid,
    ManifestMissingField,
    SignatureMissing,
    SignatureInvalid,
    SigningKeyUnknown,
    EtwIdUnknown,
    EtwIdSuspended,
    ModuleRevoked,
    PublisherRevoked,
    KeyRevoked,
    InspectorFailed,
    InspectorUnavailable,
    PermissionDeniedByAdmin,
    LicenseMissing,
    LicenseExpired,
    LicenseInvalidSignature,
    LicenseWrongDevice,
    BackupFailed,
    SandboxError,
    RegistryError,
    IncompatibleVersion,
    InternalError
}

/// <summary>
/// Anfrage an die Installationspipeline.
/// Enthält alle Informationen die AAIAS für die sequenzielle Prüfung braucht.
/// </summary>
public sealed record ModuleInstallRequest(
    /// <summary>Absoluter Pfad zur .aaix-Paketdatei (im Quarantäne-Verzeichnis).</summary>
    string          PackagePath,

    /// <summary>Woher das Paket kommt — bestimmt initiales Trust-Level.</summary>
    InstallSource   Source,

    /// <summary>
    /// Signiertes Lizenz-JWT vom Marketplace.
    /// Null bei Sideload/Developer-Installation.
    /// </summary>
    string?         LicenseToken,

    /// <summary>Geräte-ID des AAIAS-Systems (für License-Token-Bindung).</summary>
    string          DeviceId,

    /// <summary>
    /// Offline-Modus: Online-Prüfungen (ETW-API, Revocation) überspringen.
    /// Nur erlaubt wenn gecachte Revocation-Liste noch gültig ist.
    /// </summary>
    bool            AllowOffline       = false,

    /// <summary>
    /// Admin hat explizit bestätigt dass Sideload-Modul trotz fehlendem Trust aktiviert werden darf.
    /// Erfordert Admin-Session. Protokolliert im Audit-Log.
    /// </summary>
    bool            AdminOverride      = false,

    /// <summary>Developer Mode aktiv — vereinfachte Prüfkette für lokale Module.</summary>
    bool            DeveloperMode      = false);

/// <summary>Ergebnis eines einzelnen Pipeline-Schritts.</summary>
public sealed record PipelineStepResult(
    InstallPipelineStep Step,
    bool                Passed,
    InstallFailReason   FailReason = InstallFailReason.None,
    string?             Detail     = null,
    TimeSpan?           Duration   = null);

/// <summary>Vollständiges Ergebnis der Installationspipeline.</summary>
public sealed record ModuleInstallResult(
    bool                        Success,
    string                      ModuleId,
    string                      Version,
    TrustLevel                  TrustLevel,
    InstallPipelineStep         ReachedStep,
    InstallFailReason           FailReason,
    IReadOnlyList<PipelineStepResult> Steps,
    string?                     InstalledPath   = null,
    string?                     SchemaName      = null,    // DB-Schema für Modul
    string?                     BackupPath      = null,
    string?                     ErrorMessage    = null,
    bool                        RolledBack      = false)
{
    public static ModuleInstallResult Ok(
        string moduleId, string version, TrustLevel trust,
        string installedPath, string? schemaName,
        IReadOnlyList<PipelineStepResult> steps) =>
        new(true, moduleId, version, trust,
            InstallPipelineStep.Active, InstallFailReason.None,
            steps, installedPath, schemaName);

    public static ModuleInstallResult Fail(
        string moduleId, string version,
        InstallPipelineStep step, InstallFailReason reason,
        string message, IReadOnlyList<PipelineStepResult> steps,
        bool rolledBack = false) =>
        new(false, moduleId, version, TrustLevel.Quarantined,
            step, reason, steps, ErrorMessage: message, RolledBack: rolledBack);
}

/// <summary>
/// Schnittstelle eines einzelnen Pipeline-Schritts.
/// AAIAS implementiert jeden Schritt als eigene Klasse.
/// Ermöglicht einfaches Testen und Austauschen einzelner Schritte.
/// </summary>
public interface IInstallStep
{
    InstallPipelineStep Step { get; }

    Task<PipelineStepResult> ExecuteAsync(
        ModuleInstallRequest request,
        ModuleInstallContext context,
        CancellationToken    ct);

    Task RollbackAsync(
        ModuleInstallContext context,
        CancellationToken    ct);
}

/// <summary>
/// Geteilter Kontext der zwischen Pipeline-Schritten weitergereicht wird.
/// Akkumuliert Informationen die spätere Schritte brauchen.
/// </summary>
public sealed class ModuleInstallContext
{
    public string  WorkDir         { get; set; } = "";
    public string  ModuleId        { get; set; } = "";
    public string  Version         { get; set; } = "";
    public string  ExpectedHash    { get; set; } = "";
    public string? EtwId           { get; set; }
    public string? PublisherKeyId  { get; set; }
    public string? ManifestJson    { get; set; }
    public string? InspectorReport { get; set; }
    public string? SchemaName      { get; set; }
    public string? BackupPath      { get; set; }
    public string? InstalledPath   { get; set; }
    public TrustLevel TrustLevel   { get; set; } = TrustLevel.Quarantined;

    // Rechte die der Admin zur Laufzeit genehmigt oder abgelehnt hat
    public HashSet<string> ApprovedPermissions { get; } = new();
    public HashSet<string> DeniedPermissions   { get; } = new();
}

/// <summary>
/// Schnittstelle der gesamten Installationspipeline.
/// AAIAS registriert die Implementierung im DI-Container.
/// </summary>
public interface IModuleInstallPipeline
{
    Task<ModuleInstallResult> InstallAsync(
        ModuleInstallRequest request,
        IProgress<PipelineStepResult>? progress = null,
        CancellationToken              ct       = default);

    Task<ModuleInstallResult> UninstallAsync(
        string moduleId,
        bool   createBackup = true,
        CancellationToken ct = default);

    Task<ModuleInstallResult> UpdateAsync(
        ModuleInstallRequest request,
        IProgress<PipelineStepResult>? progress = null,
        CancellationToken              ct       = default);
}
