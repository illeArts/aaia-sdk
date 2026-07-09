# CHANGELOG — AAIA.Shared.Contracts

## [2.2.0] — 2026-07-09

### Added (V3 — additiv, keine Breaking Changes)

**Namespace `AAIA.Shared.Contracts.V3`** — alle neuen Typen leben in diesem Namespace. V2-Typen unberührt.

#### V3Enums.cs — neue Aufzählungstypen
- `AaiaUserRole` — Benutzerrollen (ReadOnly / ReadWrite / Admin / SuperAdmin)
- `WorkOrderStatus` — Lebenszyklus eines Work Orders (Open / InProgress / Completed / Failed / Cancelled / Archived)
- `WorkOrderPriority` — Prioritätsstufe (Low / Normal / High / Critical)
- `MemoryCategory` — Memory-Eintrags-Kategorie (Projects / Documents / Conversations / Templates / Archive / System)
- `InboxEventStatus` — Inbox-Event-Status (New / Read / Archived / Deleted)
- `ProviderStatusColor` — Anzeigefarbe Provider-Status-Chip (Green / Yellow / Red / Grey)
- `DukiScope` — DUKI-Berechtigungsumfang (OpenOnly / ReadOnly / AssistedSession / SupervisedExec / DelegatedExec / Autonomous)
- `ModuleStatus` — Installationsstatus eines Moduls (Installed / Active / Inactive / Error / Updating / Deprecated)
- `TrustLevel` — Extension Trust aus dem Marketplace (Official / Verified / Community / Unverified). **Nicht** identisch mit DeviceTrustStatus.
- `RiskLevel` — Risikostufe (Low / Medium / High / Critical)
- `ApprovalStatus` — Freigabe-Status (Pending / Approved / Denied / Expired / Cancelled)
- `ApprovalType` — Art der Freigabe-Anforderung (ExtensionEnable / ExtensionDisable / DukiAction / PermissionChange / RiskyOperation)
- `ExtensionType` — Extension-Typ (Module / Plugin / Connector / Theme / LanguagePack)
- `V3AuthMode` — Auth-Modus für V3 RoleGuard (DevelopmentHeader / LocalSession / Jwt)
- `DeviceType` — Formfaktor eines Geräts (Desktop / Mobile / Browser / Service)
- `DevicePlatform` — OS-Plattform (Windows / MacOS / Linux / iOS / Android / Web)
- `DeviceTrustStatus` — Gerätevertrauen (Pending / Trusted / Revoked / Expired). **Nicht** identisch mit TrustLevel (Extension-Marketplace).

#### V3Dtos.cs — neue Data Transfer Objects
- `HealthStatusDto` / `HealthComponentDto` — GET /api/v3/system/health
- `ProviderStatusDto` — GET /api/v3/providers
- `ModuleManifestDto` — Modul-Manifest
- `ExtensionManifestDto` — vollständiges Extension-Manifest V2.1
- `InstalledExtensionDto` — installierte Extension mit Status
- `NavigationRouteDto` — WebUI-Navigation
- `ApiErrorDto` — standardisierter Fehler über alle V3-Endpunkte
- `ApprovalRequestDto` / `AuditEntryDto` — Gatekeeper / Approval Center (V3.1c)
- `AiriSettingsDto` / `AiriSettingsPatchDto` — AIRI Companion Einstellungen (V3.2)
- `SystemConfigDto` — GET /api/v3/config
- `AuthMeDto` — GET /api/v3/auth/me (V3.3)
- **Security Pairing DTOs** (V3.2 Security):
  - `PairingSessionDto` / `CreatePairingSessionRequest` / `CompletePairingRequest`
  - `TrustedDeviceDto` / `TrustDeviceRequest` / `RevokeDeviceRequest`
  - `RemoteAccessPolicyDto` / `UpdateRemoteAccessPolicyRequest`
  - `VpnStatusDto` / `TotpStatusDto`

### Invarianten (unverändert)

- `duki.autonomous` ist kein gültiger `DukiScope`-Wert (Enum enthält `Autonomous` nur für interne Grenzfälle — nie als Standard-Permission).
- `TrustLevel` = Extension Trust (Marketplace-Lizenz-Kontext).
- `DeviceTrustStatus` = Device Trust (Security Pairing-Kontext).
- Beide Konzepte sind strikt getrennt und dürfen nicht vermischt werden.
- `AllowPublicInternet` in `RemoteAccessPolicyDto` ist immer `false` in V3.2.
- Keine Secrets, keine Private Keys in DTOs.

### Breaking Changes

**Keine.** Alle V2-Typen bleiben unverändert in `AAIA.Shared.Contracts` (ohne `.V3`-Sub-Namespace).

---

## [2.1.0]

Vorherige Version — V2 Contracts, Extensions, Marketplace, DUKI, Memory, Auth.

---

## [2.0.0]

Initial release.
