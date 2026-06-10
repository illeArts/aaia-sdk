namespace AAIA.Shared.Contracts.Dev;

/// <summary>
/// Canonical error code catalog for AAIA diagnostics.
///
/// Format: AAIA-{CATEGORY}-{NUMBER}
///
/// Categories:
///   DEV  — Developer Mode infrastructure
///   MAN  — Manifest parsing and validation
///   ASM  — Assembly loading
///   SEC  — Security and signing
///   WO   — WorkOrder execution
///   PLG  — Plugin lifecycle (client-side)
///   MOD  — Module lifecycle (server-side)
///   CFG  — Configuration
///   NET  — Network / server communication
///
/// Use these codes in DevDiagnosticsEvent.ErrorCode.
/// The ERROR_REFERENCE.md in docs/developer/ explains each code with cause and fix.
/// </summary>
public static class AaiaErrorCodes
{
    // ── Developer Mode ─────────────────────────────────────────────────────────
    public const string DevModeDisabled         = "AAIA-DEV-0001";
    public const string DevModeAlreadyActive    = "AAIA-DEV-0002";
    public const string DevEndpointNotAvailable = "AAIA-DEV-0003";

    // ── Manifest ───────────────────────────────────────────────────────────────
    public const string ManifestMissing         = "AAIA-MAN-0100";
    public const string ManifestInvalidJson     = "AAIA-MAN-0101";
    public const string ManifestFieldMissing    = "AAIA-MAN-0102";
    public const string ManifestFieldInvalid    = "AAIA-MAN-0103";
    public const string ManifestVersionMismatch = "AAIA-MAN-0104";
    public const string ManifestHostMismatch    = "AAIA-MAN-0105";
    public const string ManifestIdInvalid       = "AAIA-MAN-0106";

    // ── Assembly ───────────────────────────────────────────────────────────────
    public const string AssemblyNotFound        = "AAIA-ASM-0200";
    public const string AssemblyLoadFailed      = "AAIA-ASM-0201";
    public const string AssemblyFrameworkMismatch = "AAIA-ASM-0202";
    public const string AssemblyInterfaceMissing = "AAIA-ASM-0203";
    public const string AssemblyMultipleEntryPoints = "AAIA-ASM-0204";

    // ── Security ───────────────────────────────────────────────────────────────
    public const string SecurityUnsignedBlocked = "AAIA-SEC-0300";
    public const string SecurityPermissionDenied = "AAIA-SEC-0301";
    public const string SecurityHashMismatch    = "AAIA-SEC-0302";
    public const string SecuritySignatureInvalid = "AAIA-SEC-0303";
    public const string SecurityPublisherKeyUnknown = "AAIA-SEC-0304";

    // ── WorkOrder ──────────────────────────────────────────────────────────────
    public const string WorkOrderPayloadInvalid = "AAIA-WO-0400";
    public const string WorkOrderExecutionFailed = "AAIA-WO-0401";
    public const string WorkOrderNoHandler      = "AAIA-WO-0402";
    public const string WorkOrderTimeout        = "AAIA-WO-0403";
    public const string WorkOrderPermissionDenied = "AAIA-WO-0404";

    // ── Plugin (client-side) ──────────────────────────────────────────────────
    public const string PluginActivationFailed  = "AAIA-PLG-0500";
    public const string PluginCrashed           = "AAIA-PLG-0501";
    public const string PluginReloadFailed      = "AAIA-PLG-0502";
    public const string PluginInitFailed        = "AAIA-PLG-0503";
    public const string PluginSuspendFailed     = "AAIA-PLG-0504";
    public const string PluginStopFailed        = "AAIA-PLG-0505";
    public const string PluginNotFound          = "AAIA-PLG-0506";

    // ── Module (server-side) ──────────────────────────────────────────────────
    public const string ModuleActivationFailed  = "AAIA-MOD-0600";
    public const string ModuleLoadFailed        = "AAIA-MOD-0601";
    public const string ModuleNotFound          = "AAIA-MOD-0602";
    public const string ModuleReloadFailed      = "AAIA-MOD-0603";
    public const string ModuleDependencyMissing = "AAIA-MOD-0604";
    public const string ModuleRouteConflict     = "AAIA-MOD-0605";

    // ── Configuration ─────────────────────────────────────────────────────────
    public const string ConfigInvalid           = "AAIA-CFG-0700";
    public const string ConfigMissing           = "AAIA-CFG-0701";

    // ── Network ───────────────────────────────────────────────────────────────
    public const string NetConnectionFailed     = "AAIA-NET-0800";
    public const string NetAuthFailed           = "AAIA-NET-0801";
    public const string NetTimeout              = "AAIA-NET-0802";
}
