namespace AAIA.Shared.Contracts.Marketplace;

// ── AAIAS-Installations-Kontrakt (Phase 5.4b) ────────────────────────────────
//
// Der Module Manager sendet das heruntergeladene .aaiaext-Paket über diesen Kontrakt
// an eine lokale AAIAS-Instanz. AAIAS prüft das Paket selbst und installiert es.
//
// Endpunkt (AAIAS-Seite):
//   POST /api/extensions/install-package
//   Authorization: Bearer {aaiasToken}
//   Content-Type: application/json
//
// Sicherheitsprinzip:
//   AAIAS ist niemals passiver Empfänger — es prüft das Paket eigenständig:
//     1. ZIP öffnen + Entropie-Check (ZIP-Bomb)
//     2. aaia-manifest.json lesen + Pflichtfelder prüfen
//     3. SHA-256 des Pakets berechnen + gegen PackageSha256 prüfen (wenn befüllt)
//     4. Signatur prüfen (wenn IsSignatureVerified = true erwartet wird)
//     5. Isolation: Extension landet in AAIAS_DATA/extensions/{id}/{version}/
//     6. Lokale Registry aktualisieren
//
// ── Request ───────────────────────────────────────────────────────────────────

/// <summary>
/// Anfrage an AAIAS um ein .aaiaext-Paket aus einem lokalen Dateipfad zu installieren.
/// Der Pfad muss für den AAIAS-Prozess erreichbar sein (lokale Maschine).
/// </summary>
public sealed record InstallPackageRequest(
    /// <summary>Absoluter lokaler Pfad zur .aaiaext-Datei.</summary>
    string PackagePath,

    /// <summary>Wenn true: bestehende Version wird überschrieben.</summary>
    bool   Overwrite = true,

    /// <summary>Wenn true: auch ältere Versionen als die installierte werden akzeptiert.</summary>
    bool   AllowDowngrade = false,

    /// <summary>
    /// Optionaler erwarteter SHA-256-Hash des Pakets (hex, lowercase).
    /// AAIAS lehnt die Installation ab wenn der lokale Hash abweicht.
    /// Null = kein Vorab-Check durch AAIAS (AAIAS prüft trotzdem intern).
    /// </summary>
    string? ExpectedSha256 = null);

// ── Response ──────────────────────────────────────────────────────────────────

/// <summary>
/// Ergebnis der Installation durch AAIAS.
/// Entspricht dem AaiasInstallResult-Record im Module Manager (AaiasConnectionService.cs).
/// </summary>
public sealed record InstallPackageResponse(
    /// <summary>ExtensionId aus dem Manifest.</summary>
    string  Id,

    /// <summary>True wenn die Extension erstmals installiert wurde.</summary>
    bool    Installed,

    /// <summary>True wenn eine bestehende Version aktualisiert wurde.</summary>
    bool    Updated,

    /// <summary>True wenn die Extension direkt aktiviert ist.</summary>
    bool    Enabled,

    /// <summary>
    /// Vertrauensstatus nach der Installation:
    /// "MarketplaceVerified" | "EtwLocalVerified" | "Unverified"
    /// </summary>
    string  TrustStatus,

    /// <summary>Absoluter Pfad zum installierten Paket auf dem AAIAS-Server.</summary>
    string? PackagePath,

    /// <summary>True wenn AAIAS neu gestartet werden muss um die Extension zu laden.</summary>
    bool    RestartRequired,

    /// <summary>Version die vor der Installation aktiv war (null = Erstinstallation).</summary>
    string? PreviousVersion);

// ── Fehler-Antworten ─────────────────────────────────────────────────────────
//
// AAIAS gibt folgende HTTP-Status-Codes zurück:
//   200 OK          → Installed oder Updated
//   400 Bad Request → Paket ungültig (fehlerhaftes Manifest, Hash-Mismatch, ZIP-Bomb)
//   401 Unauthorized→ Kein oder ungültiger Bearer-Token
//   403 Forbidden   → Nicht genug Rechte (z.B. Extension nicht erlaubt)
//   409 Conflict    → Version existiert bereits + Overwrite=false
//   500 Internal    → Dateisystem-Fehler, Schreibfehler
//
// Alle Fehler-Antworten verwenden folgendes JSON-Format:
//   { "error": "...", "detail": "..." }
