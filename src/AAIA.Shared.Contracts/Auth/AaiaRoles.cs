namespace AAIA.Shared.Contracts.Auth;

/// <summary>
/// Rollenkonstanten für das AAIA-Plattform-Rollenmodell.
///
/// Hierarchie (absteigend):
///   Owner      — Plattformbesitzer. Volle Kontrolle über Geld, Lizenzen, Trust, Betrieb.
///   Admin      — Technische Verwaltung (z.B. Verifikation, Support). Kein Geld-/MoR-Zugriff.
///   Partner    — Geprüfter Entwickler/Publisher. Marketplace-Publish-Zugriff.
///   Developer  — Normaler ETW-Entwickler. Eigene Module, eigene Lizenzen.
///   User       — Endkunde/Käufer. Nur Lesezugriff auf eigene Lizenzen.
///
/// Rollenstrings werden 1:1 als JWT-Claim (ClaimTypes.Role) gesetzt — JwtService.CreateToken().
/// NIEMALS Magic-Strings in Controllern — immer diese Konstanten + AaiaAuthPolicies verwenden.
///
/// WICHTIG — Mapping zu DeveloperRole (DB-Enum):
///   AaiaRoles.Owner = "Owner" entspricht DeveloperRole.Owner.ToString()
///   Alle anderen AaiaRoles-Strings müssen, wenn sie als JWT-Rolle vergeben werden,
///   auch als DeveloperRole-Enum-Wert existieren (oder als Custom-Claim gesetzt werden).
///   Derzeit existiert in DeveloperRole: Community, VerifiedDeveloper, Professional,
///   TrustedDeveloper, Owner. Die Strings Admin/Partner/Developer sind reserviert für
///   zukünftige Erweiterungen des DeveloperRole-Enums.
/// </summary>
public static class AaiaRoles
{
    /// <summary>Plattformbesitzer. Vollzugriff auf alle Admin-Funktionen inkl. MoR, Geld, Trust.</summary>
    public const string Owner = "Owner";

    /// <summary>Technische Verwaltung. Kein Zugriff auf MoR-Mappings, Zahlungsflüsse.</summary>
    public const string Admin = "Admin";

    /// <summary>Geprüfter Entwickler/Publisher. Darf Module veröffentlichen.</summary>
    public const string Partner = "Partner";

    /// <summary>Normaler ETW-Entwickler. Eigene Module und Lizenzen.</summary>
    public const string Developer = "Developer";

    /// <summary>Endkunde. Lesezugriff auf eigene Lizenzen und Downloads.</summary>
    public const string User = "User";

    // ── MoR-Provider-Strings (für MorProductMapping.Provider) ────────────────
    // Hierher, weil Provider-Validierung in Controller + SDK identisch sein muss.

    /// <summary>Lemon Squeezy als Merchant of Record.</summary>
    public const string ProviderLemonSqueezy = "LemonSqueezy";

    /// <summary>Paddle als Merchant of Record.</summary>
    public const string ProviderPaddle = "Paddle";

    /// <summary>Alle gültigen MoR-Provider-Strings.</summary>
    public static readonly IReadOnlySet<string> ValidMorProviders =
        new HashSet<string>(StringComparer.Ordinal) { ProviderLemonSqueezy, ProviderPaddle };
}

/// <summary>
/// Namen der Authorization-Policies.
/// In Program.cs via AddAuthorization(options => ...) registriert.
/// Im Controller via [Authorize(Policy = AaiaAuthPolicies.OwnerOnly)] verwendet.
/// </summary>
public static class AaiaAuthPolicies
{
    /// <summary>
    /// Nur Owner.
    /// Für: MoR-Mappings, Webhook-Secrets, Lizenz-Ablauf-Trigger, Revocation.
    /// Alles was direkten Einfluss auf Geld, Vertrauen oder Systemintegrität hat.
    /// </summary>
    public const string OwnerOnly = "OwnerOnly";

    /// <summary>
    /// Owner oder Admin.
    /// Für: technische Admin-Operationen ohne direkten Geld-/Trust-Einfluss
    /// (z.B. Support-Tools, Diagnose-Endpunkte, Registry-Moderation).
    /// </summary>
    public const string OwnerOrAdmin = "OwnerOrAdmin";

    /// <summary>
    /// Owner, Admin oder Partner.
    /// Für: erweiterte Entwickler-Operationen (z.B. Publish-Freigabe-Anträge).
    /// </summary>
    public const string OwnerOrAdminOrPartner = "OwnerOrAdminOrPartner";
}
