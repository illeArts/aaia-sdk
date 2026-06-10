using System.Text.RegularExpressions;

namespace AAIA.Shared.Contracts.Extensions;

public enum AaiaExtensionHost
{
    AAIAS,
    AAIAC
}

public enum AaiaExtensionKind
{
    Module,
    Plugin,
    Connector,
    Tool
}

public enum AaiaExtensionPermissionScope
{
    ServerRead,
    ServerWrite,
    ServerAdmin,
    ClientLocalRead,
    ClientLocalWrite,
    ClientLocalAdmin,
    NetworkAccess,
    SecretAccess,
    DukiAction,
    AuditWrite
}

public enum AaiaExtensionRiskLevel
{
    Green,
    Yellow,
    Orange,
    Red
}

/// <summary>
/// Klasse eines Moduls/Plugins — die wichtigste Einteilung.
///
/// Standard = plattformneutral.
/// Ausnahme = plattformspezifisch, muss explizit deklariert werden.
///
///   ServerModule  → Servermodule MÜSSEN plattformneutral sein (platforms: ["all"])
///   ClientPlugin  → Clientplugins SOLLEN plattformneutral sein; Ausnahmen erlaubt
///   SystemPlugin  → Systemplugins DÜRFEN plattformspezifisch sein (Drucker, Scanner, Mikrofon, Desktop)
/// </summary>
public enum AaiaPluginClass
{
    /// <summary>Läuft auf AAIAS, plattformunabhängig. Pflichtfeld: platforms = ["all"].</summary>
    ServerModule,

    /// <summary>Erweitert AAIAC-UI/Logik. Soll plattformneutral sein; Ausnahmen möglich.</summary>
    ClientPlugin,

    /// <summary>Greift auf Systemressourcen zu (Drucker, Scanner, Mikrofon, Desktop-Vision, Hotkeys).
    /// Darf plattformspezifisch sein — muss platforms explizit deklarieren.</summary>
    SystemPlugin
}

/// <summary>Zugriffsstufe auf Systemressourcen.</summary>
public enum AaiaSystemAccessLevel
{
    /// <summary>Kein direkter Systemzugriff.</summary>
    None,

    /// <summary>Lesender Zugriff (Clipboard lesen, Systeminfo, Sensoren).</summary>
    ReadOnly,

    /// <summary>Aktiver Zugriff (Mikrofon, Kamera, Desktop-Beobachtung, Hotkeys).</summary>
    Active,

    /// <summary>Steuernder Zugriff (Maussteuerung, Fenstersteuerung, Desktop-Vision+Aktion, DUKI-Pflicht).</summary>
    Control
}



public sealed record AaiaExtensionManifestDto(
    string Id,
    string DisplayName,
    string Version,
    AaiaExtensionHost Host,
    AaiaExtensionKind Kind,
    string PublisherId,
    string Description,
    IReadOnlyList<AaiaExtensionPermissionDto> Permissions,
    IReadOnlyList<AaiaExtensionRouteDto> Routes,
    IReadOnlyList<string> RequiredSecrets,
    IReadOnlyList<string> NetworkTargets,
    IReadOnlyList<string> SupportedPlatforms,
    AaiaExtensionSigningDto? Signing,
    /// <summary>Pflichtfeld ab v1.0. Bestimmt Plattformanforderungen und Sicherheitsregeln.</summary>
    AaiaPluginClass? PluginClass = null,
    /// <summary>Zugriffsstufe auf Systemressourcen. Pflichtfeld für SystemPlugin.</summary>
    AaiaSystemAccessLevel? SystemAccessLevel = null,
    /// <summary>Minimale AAIAS-Version (z. B. "0.5.0"). Null = keine Anforderung.</summary>
    string? MinAaiasVersion = null,
    /// <summary>Minimale AAIAC-Version. Null = keine Anforderung.</summary>
    string? MinAaiacVersion = null);

public sealed record AaiaExtensionPermissionDto(
    AaiaExtensionPermissionScope Scope,
    string Reason,
    AaiaExtensionRiskLevel Risk,
    bool RequiresDuki,
    bool RequiresAdminApproval);

public sealed record AaiaExtensionRouteDto(
    string Method,
    string Route,
    AaiaExtensionRiskLevel Risk,
    bool RequiresAuth,
    bool RequiresAdmin,
    bool RequiresDuki);

public sealed record AaiaExtensionSigningDto(
    string? PublisherKeyId,
    string PackageSha256,
    string? Signature,
    string? SignatureAlgorithm);

public static class AaiaExtensionManifestRules
{
    private static readonly Regex IdentifierPattern =
        new("^[a-z0-9][a-z0-9._-]{2,127}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex SecretNamePattern =
        new("^[A-Z][A-Z0-9_]{2,127}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly HashSet<string> AllowedHttpMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        "GET",
        "POST",
        "PUT",
        "PATCH",
        "DELETE"
    };

    private static readonly HashSet<string> MutatingHttpMethods = new(StringComparer.OrdinalIgnoreCase)
    {
        "POST",
        "PUT",
        "PATCH",
        "DELETE"
    };

    private static readonly HashSet<string> SupportedPlatformIds = new(StringComparer.OrdinalIgnoreCase)
    {
        "all",        // plattformneutral (Pflicht für ServerModule)
        "windows",
        "macos",
        "linux",
        "ios",
        "ipados",
        "android"
    };

    public static IReadOnlyList<string> Validate(AaiaExtensionManifestDto manifest)
    {
        var errors = new List<string>();
        var permissions = manifest.Permissions ?? Array.Empty<AaiaExtensionPermissionDto>();
        var routes = manifest.Routes ?? Array.Empty<AaiaExtensionRouteDto>();
        var requiredSecrets = manifest.RequiredSecrets ?? Array.Empty<string>();
        var networkTargets = manifest.NetworkTargets ?? Array.Empty<string>();
        var supportedPlatforms = manifest.SupportedPlatforms ?? Array.Empty<string>();

        Require(manifest.Id, "id");
        Require(manifest.DisplayName, "displayName");
        Require(manifest.Version, "version");
        Require(manifest.PublisherId, "publisherId");
        Require(manifest.Description, "description");

        if (!string.IsNullOrWhiteSpace(manifest.Id) && !IdentifierPattern.IsMatch(manifest.Id))
            errors.Add("id must use 3-128 lowercase letters, digits, dots, hyphens, or underscores, and start with a letter or digit.");

        if (!string.IsNullOrWhiteSpace(manifest.PublisherId) && !IdentifierPattern.IsMatch(manifest.PublisherId))
            errors.Add("publisherId must use 3-128 lowercase letters, digits, dots, hyphens, or underscores, and start with a letter or digit.");

        if (!string.IsNullOrWhiteSpace(manifest.Version) &&
            !Version.TryParse(StripSemVerSuffix(manifest.Version), out _))
        {
            errors.Add("version must be a valid numeric version or semver string.");
        }

        if (supportedPlatforms.Count == 0)
            errors.Add("supportedPlatforms must contain at least one platform.");

        foreach (var platform in supportedPlatforms)
        {
            if (string.IsNullOrWhiteSpace(platform) || !SupportedPlatformIds.Contains(platform))
                errors.Add($"supportedPlatforms contains unsupported platform '{platform}'.");
        }

        AddDuplicateErrors(supportedPlatforms, "supportedPlatforms", errors);
        AddDuplicateErrors(requiredSecrets, "requiredSecrets", errors);
        AddDuplicateErrors(networkTargets, "networkTargets", errors);

        if (permissions.Any(p => p.Scope is AaiaExtensionPermissionScope.ServerAdmin &&
                                 manifest.Host is AaiaExtensionHost.AAIAC))
        {
            errors.Add("AAIAC extensions must not request ServerAdmin permission.");
        }

        if (permissions.Any(p => IsClientLocalScope(p.Scope) && manifest.Host is AaiaExtensionHost.AAIAS))
        {
            errors.Add("AAIAS extensions must not request ClientLocal permissions; local-client permissions belong to AAIAC extensions.");
        }

        if (permissions.Any(p => p.Risk is AaiaExtensionRiskLevel.Orange or AaiaExtensionRiskLevel.Red &&
                                 !p.RequiresDuki))
        {
            errors.Add("Orange and red permissions must require DUKI.");
        }

        if (permissions.Any(p => p.Risk is AaiaExtensionRiskLevel.Orange or AaiaExtensionRiskLevel.Red &&
                                 !p.RequiresAdminApproval))
        {
            errors.Add("Orange and red permissions must require admin approval.");
        }

        if (permissions.Any(p => string.IsNullOrWhiteSpace(p.Reason)))
            errors.Add("Every permission must declare a reason.");

        var duplicatePermissionScopes = permissions
            .GroupBy(p => p.Scope)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key.ToString());
        foreach (var scope in duplicatePermissionScopes)
            errors.Add($"permissions contains duplicate scope '{scope}'.");

        if (permissions.Any(p => p.Scope is AaiaExtensionPermissionScope.SecretAccess) &&
            requiredSecrets.Count == 0)
        {
            errors.Add("SecretAccess permission requires at least one requiredSecrets entry.");
        }

        if (permissions.Any(p => p.Scope is AaiaExtensionPermissionScope.NetworkAccess) &&
            networkTargets.Count == 0)
        {
            errors.Add("NetworkAccess permission requires at least one networkTargets entry.");
        }

        foreach (var secret in requiredSecrets)
        {
            if (string.IsNullOrWhiteSpace(secret) || !SecretNamePattern.IsMatch(secret))
                errors.Add($"requiredSecrets contains invalid secret name '{secret}'.");
        }

        foreach (var target in networkTargets)
        {
            if (!Uri.TryCreate(target, UriKind.Absolute, out var uri) ||
                uri.Scheme is not ("http" or "https"))
            {
                errors.Add($"networkTargets contains invalid HTTP/HTTPS target '{target}'.");
            }
        }

        foreach (var route in routes)
        {
            if (string.IsNullOrWhiteSpace(route.Method) || !AllowedHttpMethods.Contains(route.Method))
                errors.Add($"route '{route.Route}' uses unsupported HTTP method '{route.Method}'.");

            if (string.IsNullOrWhiteSpace(route.Route) ||
                !route.Route.StartsWith("/api/", StringComparison.OrdinalIgnoreCase) ||
                route.Route.Contains("..", StringComparison.Ordinal))
            {
                errors.Add($"route '{route.Route}' must be an absolute /api/ route without traversal segments.");
            }

            if (route.Route.StartsWith("/api/admin", StringComparison.OrdinalIgnoreCase) &&
                !route.RequiresAdmin)
            {
                errors.Add("Admin routes must require admin authorization.");
            }

            if ((route.RequiresAdmin || route.RequiresDuki) && !route.RequiresAuth)
                errors.Add($"route '{route.Method} {route.Route}' must require auth when it requires admin or DUKI.");

            if (MutatingHttpMethods.Contains(route.Method) && !route.RequiresAuth)
                errors.Add($"mutating route '{route.Method} {route.Route}' must require auth.");

            if (MutatingHttpMethods.Contains(route.Method) &&
                route.Risk is AaiaExtensionRiskLevel.Orange or AaiaExtensionRiskLevel.Red &&
                !route.RequiresDuki)
            {
                errors.Add($"orange/red mutating route '{route.Method} {route.Route}' must require DUKI.");
            }

            if (route.Risk is AaiaExtensionRiskLevel.Red && !route.RequiresDuki)
                errors.Add($"red route '{route.Method} {route.Route}' must require DUKI.");
        }

        var duplicateRoutes = routes
            .GroupBy(r => $"{r.Method.ToUpperInvariant()} {r.Route}", StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);
        foreach (var route in duplicateRoutes)
            errors.Add($"routes contains duplicate route '{route}'.");

        if (manifest.Signing is not null)
        {
            var hasSignature = !string.IsNullOrWhiteSpace(manifest.Signing.Signature);
            if (hasSignature && string.IsNullOrWhiteSpace(manifest.Signing.PublisherKeyId))
                errors.Add("signing.publisherKeyId is required when signing is present.");
            if (string.IsNullOrWhiteSpace(manifest.Signing.PackageSha256) ||
                manifest.Signing.PackageSha256.Length != 64 ||
                !manifest.Signing.PackageSha256.All(Uri.IsHexDigit))
            {
                errors.Add("signing.packageSha256 must be a 64 character hex SHA256 hash.");
            }
        }


        // ── PluginClass-Regeln (Plattform-Neutralität) ─────────────────────────
        //
        // Kernregel: Standard = plattformneutral. Ausnahme = plattformspezifisch.
        //   ServerModule  → MUSS platforms=["all"] haben
        //   ClientPlugin  → SOLL ["all"] haben; Ausnahme erlaubt wenn begründet
        //   SystemPlugin  → DARF plattformspezifisch sein, muss aber deklarieren
        //
        // Wenn PluginClass noch nicht gesetzt: Warnung, aber kein Fehler (Kompatibilität)

        if (manifest.PluginClass is { } pluginClass)
        {
            switch (pluginClass)
            {
                case AaiaPluginClass.ServerModule:
                    // ServerModule MÜSSEN plattformneutral sein
                    if (supportedPlatforms.Count != 1 || !supportedPlatforms[0].Equals("all", StringComparison.OrdinalIgnoreCase))
                        errors.Add("ServerModule muss supportedPlatforms = [\"all\"] deklarieren. " +
                                   "Servermodule laufen plattformunabhängig — kein betriebssystemspezifischer Code erlaubt.");

                    if (manifest.Host is not AaiaExtensionHost.AAIAS)
                        errors.Add("ServerModule muss Host = AAIAS deklarieren.");

                    if (manifest.SystemAccessLevel is not null and not AaiaSystemAccessLevel.None)
                        errors.Add("ServerModule darf keinen SystemAccessLevel > None deklarieren. " +
                                   "Systemzugriff gehört in ein SystemPlugin.");
                    break;

                case AaiaPluginClass.ClientPlugin:
                    // ClientPlugin SOLL plattformneutral sein — Warnung wenn nicht
                    if (supportedPlatforms.Count > 0 &&
                        !supportedPlatforms.Any(p => p.Equals("all", StringComparison.OrdinalIgnoreCase)))
                    {
                        // Plattformspezifische ClientPlugins sind erlaubt, aber müssen deklarieren
                        // Keine Error — nur Hinweis dass es Kompatibilität einschränkt
                        // (Architektur-Entscheidung: Warnungen werden im UI angezeigt)
                    }

                    if (manifest.SystemAccessLevel is AaiaSystemAccessLevel.Control or AaiaSystemAccessLevel.Active)
                    {
                        errors.Add("ClientPlugin mit SystemAccessLevel Active/Control muss als SystemPlugin deklariert sein. " +
                                   "Aktiver Systemzugriff gehört nicht in ein normales ClientPlugin.");
                    }
                    break;

                case AaiaPluginClass.SystemPlugin:
                    // SystemPlugin DARF plattformspezifisch sein, muss aber explizit deklarieren
                    if (supportedPlatforms.Count == 0)
                        errors.Add("SystemPlugin muss supportedPlatforms explizit deklarieren (z. B. [\"windows\"] oder [\"all\"]).");

                    if (manifest.SystemAccessLevel is null)
                        errors.Add("SystemPlugin muss SystemAccessLevel deklarieren. " +
                                   "Erlaubt: ReadOnly, Active, Control.");

                    if (manifest.SystemAccessLevel is AaiaSystemAccessLevel.Control)
                    {
                        // Control-Zugriff (Maussteuerung, Desktop-Vision+Aktion) ist hochriskant
                        var hasOrangeOrRed = permissions.Any(p =>
                            p.Risk is AaiaExtensionRiskLevel.Orange or AaiaExtensionRiskLevel.Red);
                        if (!hasOrangeOrRed)
                            errors.Add("SystemPlugin mit SystemAccessLevel Control muss mindestens eine Orange/Red Permission deklarieren.");
                    }
                    break;
            }
        }

        // MinVersion-Format prüfen
        if (!string.IsNullOrWhiteSpace(manifest.MinAaiasVersion) &&
            !System.Version.TryParse(StripSemVerSuffix(manifest.MinAaiasVersion), out _))
        {
            errors.Add("minAaiasVersion muss ein gültiges Versionsformat haben (z. B. \"0.5.0\").");
        }

        if (!string.IsNullOrWhiteSpace(manifest.MinAaiacVersion) &&
            !System.Version.TryParse(StripSemVerSuffix(manifest.MinAaiacVersion), out _))
        {
            errors.Add("minAaiacVersion muss ein gültiges Versionsformat haben (z. B. \"0.5.0\").");
        }

        return errors;

        void Require(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                errors.Add($"{name} is required.");
        }
    }

    private static bool IsClientLocalScope(AaiaExtensionPermissionScope scope) =>
        scope is AaiaExtensionPermissionScope.ClientLocalRead or
            AaiaExtensionPermissionScope.ClientLocalWrite or
            AaiaExtensionPermissionScope.ClientLocalAdmin;

    private static string StripSemVerSuffix(string version)
    {
        var plus = version.IndexOf('+', StringComparison.Ordinal);
        if (plus >= 0)
            version = version[..plus];

        var dash = version.IndexOf('-', StringComparison.Ordinal);
        if (dash >= 0)
            version = version[..dash];

        return version;
    }

    private static void AddDuplicateErrors(IEnumerable<string> values, string fieldName, List<string> errors)
    {
        var duplicates = values
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .GroupBy(v => v, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        foreach (var duplicate in duplicates)
            errors.Add($"{fieldName} contains duplicate value '{duplicate}'.")