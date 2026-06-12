# AAIA Modul-Sicherheitsarchitektur

**Version:** 2.1.0  
**Stand:** 2026-06-11

---

## Grundprinzip

> **Die Installation ist nicht der Schutzpunkt. Die Ausführung ist der Schutzpunkt.**

Jeder darf technisch ein Modul installieren. Aber nicht jedes installierte Modul bekommt automatisch Vertrauen, Netzwerkzugriff, Datenbankrechte, Updates oder Sichtbarkeit im System.

Lizenz ≠ Sicherheit ≠ Vertrauen. Diese drei Ebenen sind bewusst getrennt.

---

## Die drei Installationswege

### 1. Marketplace-Installation (Standardweg für Benutzer)

```
AAIAC → AAIAS → Marketplace API → Paket herunterladen → Pipeline
```

- Paket enthält: Modul-ZIP + `aaia-extension.json` + `aaia-signature.sig` + `aaia-hash.sha256`
- **Lizenz kommt separat:** Als signiertes RS256-JWT vom Marketplace API, gebunden an `ETW-ID + DeviceId + ModuleId`
- Die Lizenz-Datei ist **nicht** im Paket — ein kopiertes ZIP hat keine Lizenz
- Erreichtes Trust-Level: `Verified`

### 2. Offline-Paket (signiert, ohne Online-Verifikation)

```
Offline-ZIP mit AAIA-Signatur + Offline-Lizenz-JWT → Pipeline
```

- Signatur wird mathematisch lokal geprüft (kein API-Call nötig)
- Revocation-Cache wird geprüft (max. 24h alt)
- Erreichtes Trust-Level: `OfflineVerified`
- Kein Auto-Update ohne Online-Verbindung

### 3. Sideload / Developer-Installation

```
Lokales ZIP → Pipeline (vereinfacht) → Eingeschränkte Rechte
```

- Kein Netzwerkzugriff, kein DB-Schreibzugriff, nur Sandbox-Dateisystem
- Manueller Admin-Dialog erforderlich
- Erreichtes Trust-Level: `LocalUnverified`
- Developer Mode muss explizit aktiviert sein

---

## Trust-Level Zustandsmaschine

```
Quarantined
    │
    ├──(Hash + Manifest OK)──► LocalUnverified
    │                               │
    │                    (Admin-Override für Sideload)
    │                               │
    ├──(Signatur OK, offline)──► OfflineVerified
    │                               │
    └──(Signatur + ETW + Revocation online OK)──► Verified
                                    │
                         (Revocation-Liste)
                                    │
                                ▼ Revoked
                         (Inspector fail)
                                    │
                                ▼ Blocked
```

| Trust-Level     | Netzwerk | DB-Write | Shell | DukiDirect | AutoUpdate |
|-----------------|----------|----------|-------|------------|------------|
| Quarantined     | ✗        | ✗        | ✗     | ✗          | ✗          |
| LocalUnverified | ✗        | ✗        | ✗     | ✗          | ✗          |
| OfflineVerified | ✓        | ✓        | ✗     | ✗          | ✗          |
| Verified        | ✓        | ✓        | ✗     | ✗          | ✓*         |
| Revoked         | ✗        | ✗        | ✗     | ✗          | ✗          |
| Blocked         | ✗        | ✗        | ✗     | ✗          | ✗          |

*AutoUpdate nur wenn `InstallSource == Marketplace`

**`DukiDirect` ist nie automatisch.** Weder für Marketplace-Module noch für verifizierte Module.  
Shell-Ausführung ist grundsätzlich verboten — auch für `Verified`-Module.

---

## Installationspipeline (sequenziell)

```
Received → Extracted → HashVerified → ManifestVerified →
SignatureVerified → EtwVerified → RevocationChecked →
InspectorPassed → RightsApproved → LicenseVerified →
BackupCreated → SandboxInstalled → RegistryEntryCreated →
AuditWritten → Active
```

Jeder Schritt kann die Pipeline stoppen. Ab `SandboxInstalled` ist Rollback möglich.  
Die Pipeline-Contracts liegen in `AAIA.Shared.Contracts.Security.ModuleInstallPipeline`.  
Die Implementierung liegt in AAIAS (`ModuleInstallPipeline.cs`).

---

## Revocation

AAIAS pollt täglich `GET /api/revocations` und cached das Ergebnis lokal.

- **Online:** Vollständige signierte Liste, 24h HTTP-Cache
- **Delta-Updates:** `GET /api/revocations/delta?since=42` — nur neue Einträge
- **Offline:** Cache wird bis zu seinem `ValidUntil`-Datum akzeptiert
- **Signatur:** RS256 mit Marketplace-Private-Key — AAIAS verifiziert mit Public Key

Was kann revoked werden:
- `ModuleId` — spezifisches Modul
- `EtwId` — alle Module eines Entwicklers
- `KeyId` — alle mit einem bestimmten Key signierten Module

Gründe: `SecurityVulnerability`, `MaliciousCode`, `PolicyViolation`, `DeveloperSuspended`, `KeyCompromised`, `LicenseViolation`, `RequestedByDeveloper`

---

## Lizenz-JWT

Lizenzen werden als signierte RS256-JWTs ausgestellt — **nicht als Datei im Paket**.

**Claims:**
```json
{
  "sub":        "ETW-000042",
  "module_id":  "com.example.fritzbox",
  "device_id":  "aaias-device-uuid",
  "etw_id":     "ETW-000042",
  "license_id": "123",
  "jti":        "unique-token-id",
  "iat":        1749600000,
  "exp":        1757376000
}
```

- Token ist an `DeviceId` gebunden → Paket-Weitergabe umgeht Lizenz nicht
- AAIAS verifiziert offline mit gecachtem Public Key (`GET /api/marketplace/public-key`)
- Widerruf: `jti` wird auf Revocation-Liste gesetzt → Token ungültig ohne Re-Install
- Standard-Ablauf: 90 Tage → danach muss AAIAS online ein neues Token holen

**Endpoint:** `POST /api/marketplace/licenses/token`  
Authentifizierung: JWT des Käufers (ETW-ID im Token muss mit `BuyerEtwId` übereinstimmen)

---

## Rechtemodell (Manifest-Deklaration)

Module deklarieren benötigte Rechte im Manifest:

```json
{
  "permissions": [
    { "scope": "NetworkAccess",      "reason": "FritzBox API", "risk": "Yellow" },
    { "scope": "ServerRead",         "reason": "Kontaktliste", "risk": "Green"  },
    { "scope": "ServerWrite",        "reason": "Anruflog",     "risk": "Yellow", "requiresDuki": true }
  ]
}
```

**Gefährliche Rechte — nur mit Admin-Bestätigung + Audit:**

| Permission              | Anforderung                          |
|-------------------------|--------------------------------------|
| `DukiAction`            | Verified + Admin-Bestätigung         |
| `DukiDirect`            | Verboten für Drittanbieter-Module    |
| `ShellExecute`          | Verboten                             |
| `SystemModify`          | Verboten                             |
| `NetworkExternal`       | Verified + Admin-Bestätigung         |
| `DatabaseAdmin`         | Verboten für Drittanbieter-Module    |

**DB-Isolation:**  
Jedes Modul bekommt bei Installation ein eigenes PostgreSQL-Schema (`schema_module_{module_id}`).  
AAIAS lehnt Direct-SQL ab — alle DB-Operationen laufen über einen API-Interceptor der das Schema erzwingt.

---

## AAIAC darf nicht direkt installieren

```
AAIAC: Benutzer klickt „Installieren"
  ↓
AAIAS: empfängt Installationsanfrage
AAIAS: führt vollständige Pipeline aus
AAIAS: gibt Ergebnis zurück an AAIAC
  ↓
AAIAC: zeigt Status an
```

AAIAC ist der Client. Er darf anzeigen, auswählen und Anfragen stellen.  
Die Entscheidung trifft immer AAIAS.

---

## Git-Dokumentation

Die Installationsanleitung auf Git wird **nicht entfernt**, aber umbenannt:

- Alt: "So installierst du ein Modul direkt"
- Neu: "So entwickelst du ein Modul im Developer Mode"

Klare Trennung in der Doku:

```
Für Entwickler:  lokales Testen, Manifest erstellen, Signatur-Test, Packen, Veröffentlichen
Für Benutzer:    Installation über AAIA Marketplace / Module Manager
```

---

## Modulstatus-Anzeige (AAIAS + AAIAC)

Jedes Modul wird mit Status angezeigt:

```
AAIA FritzBox Modul
Status:    ✅ Verified
Quelle:    AAIA Marketplace
ETW-ID:    ETW-00042
Signatur:  gültig
Inspector: bestanden
Lizenz:    aktiv (bis 2026-09-10)
Rechte:    Netzwerk, Router API, Telefonbuch
```

Oder:

```
Unbekanntes Börsenmodul
Status:    ⚠ LocalUnverified
Quelle:    Sideload
Signatur:  fehlt
Inspector: nicht geprüft
Rechte:    Netzwerk, Trading API, Shell ← blockiert
Aktion:    Nur nach Admin-Freigabe mit eingeschränkten Rechten
```

---

## Implementierungsstand

| Komponente                          | Status     |
|-------------------------------------|------------|
| `TrustLevel`, `InstallSource`       | ✅ Contracts |
| `ModuleTrustProfile`                | ✅ Contracts |
| `ModuleInstallPipeline` (Interface) | ✅ Contracts |
| Revocation-List API                 | ✅ aaia-marketplace-api |
| Revocation-Controller               | ✅ aaia-marketplace-api |
| License-JWT Service                 | ✅ aaia-marketplace-api |
| License-Token-Endpoint              | ✅ aaia-marketplace-api |
| Marketplace Public Key Endpoint     | ✅ aaia-marketplace-api |
| Pipeline-Implementierung (AAIAS)    | 🔲 Nächster Schritt |
| DB-Schema-Isolation (AAIAS)         | 🔲 Nächster Schritt |
| Trust-Level-Anzeige (AAIAC)         | 🔲 Nächster Schritt |
| Revocation-Cache (AAIAS)            | 🔲 Nächster Schritt |
| License-Token-Verifikation (AAIAS)  | 🔲 Nächster Schritt |
