namespace AAIA.Shared.Contracts.Recovery;

// ─────────────────────────────────────────────────────────────────────────────
// Szenario 1 — Passwort vergessen
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Startet eine Recovery-Session mit dem Recovery-Key.
/// Nur von localhost erreichbar (physischer Zugang erforderlich).
/// </summary>
public record StartPasswordRecoveryRequest(
    string RecoveryKey);   // Hex-Seed aus Ersteinrichtung

public record StartPasswordRecoveryResponse(
    string SessionToken,   // Zeitbegrenzt (15 Min)
    DateTimeOffset ExpiresAt);

public record ResetPasswordRequest(
    string SessionToken,
    string NewPassword,    // min. 12 Zeichen
    string NewPasswordConfirm);

// ─────────────────────────────────────────────────────────────────────────────
// Szenario 2 — Gerät verloren / neues Gerät koppeln
// ─────────────────────────────────────────────────────────────────────────────

public record PairNewDeviceRequest(
    string RecoveryKey,
    string DeviceName,
    string DevicePublicKey); // Base64-kodierter öffentlicher Schlüssel des neuen Geräts

public record PairNewDeviceResponse(
    string PairingCode,    // kurzlebig, nur einmal nutzbar
    DateTimeOffset ExpiresAt);

// ─────────────────────────────────────────────────────────────────────────────
// Szenario 3 — Notfall-Recovery (2-von-3)
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Startet den Notfall-Modus.
/// Benötigt 2 von 3: RecoveryKey + VendorSignatur + Localhost-Zugang.
/// Der Localhost-Zugang ist implizit (Middleware prüft RemoteIpAddress).
/// </summary>
public record StartEmergencyRecoveryRequest(
    string RecoveryKey,         // Faktor 1: Recovery-Seed des Kunden
    string VendorSignature,     // Faktor 2: ECDSA-Signatur des Anbieters (Base64)
    string VendorTimestamp,     // ISO8601 — Signatur gilt nur 5 Min
    string SupportTicketId);    // Für Audit-Log (z.B. "TICKET-2026-001")

public record StartEmergencyRecoveryResponse(
    string SessionToken,
    DateTimeOffset ExpiresAt,
    string AuditEntryId);       // Referenz für Revisionsbericht

public record EmergencyResetAdminRequest(
    string SessionToken,
    string NewAdminUsername,
    string NewAdminPassword);

// ─────────────────────────────────────────────────────────────────────────────
// Recovery-Seed Setup (bei Ersteinrichtung)
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Wird einmalig beim ersten SuperAdmin-Setup zurückgegeben.
/// Danach wird nur noch der Hash gespeichert — der Klartext ist unwiederbringlich weg.
/// </summary>
public record RecoverySeedResponse(
    string SeedHex,            // 64 Hex-Zeichen = 32 Byte
    string SeedFormatted,      // "AABB-CCDD-EEFF-..." für einfaches Aufschreiben
    DateTimeOffset GeneratedAt,
    string Warning);           // "Diesen Schlüssel jetzt ausdrucken. Er wird nie wieder angezeigt."

// ─────────────────────────────────────────────────────────────────────────────
// Audit-Log
// ─────────────────────────────────────────────────────────────────────────────

public record RecoveryAuditEntry(
    string   Id,
    DateTimeOffset Timestamp,
    string   EventType,        // "PASSWORD_RESET" | "EMERGENCY_START" | "ADMIN_RESET" | ...
    string   Actor,            // "customer" | "vendor" | "system"
    string   Details,
    bool     Success,
    string   Checksum);        // SHA256(prev_checksum + id + timestamp + eventtype + details)

public record RecoveryAuditLogResponse(
    IReadOnlyList<RecoveryAuditEntry> Entries,
    string ChainValid);        // "OK" oder "TAMPERED:entry_id"

// ─────────────────────────────────────────────────────────────────────────────
// Recovery-Status
// ─────────────────────────────────────────────────────────────────────────────

public record RecoveryStatusResponse(
    bool   SeedConfigured,
    bool   EmergencyModeActive,
    int    AuditEntryCount,
    DateTimeOffset? LastRecoveryEvent);
