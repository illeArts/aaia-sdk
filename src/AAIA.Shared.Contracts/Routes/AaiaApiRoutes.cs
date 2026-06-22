namespace AAIA.Shared.Contracts.Routes;

public static class AaiaApiRoutes
{
    public const string Health = "/health";

    public static class Server
    {
        public const string Info = "/api/server/info";
        public const string Status = "/api/server/status";
        public const string Capabilities = "/api/server/capabilities";
        public const string Version = "/api/server/version";
        public const string Readiness = "/api/server/readiness";
        public const string EvaluateModels = "/api/server/readiness/evaluate-models";
    }

    public static class Bootstrap
    {
        public const string Status = "/api/bootstrap/status";
        public const string Detect = "/api/bootstrap/detect";
        public const string Setup = "/api/bootstrap/setup";
        public const string Admin = "/api/bootstrap/admin";
    }

    public static class Setup
    {
        public const string Readiness = "/api/setup/readiness";
        public const string RepairPlan = "/api/setup/repair-plan";
        public const string Preflight = "/api/setup/preflight";
        public const string Repair = "/api/setup/repair";
    }

    public static class Auth
    {
        public const string Login = "/api/auth/login";
        public const string TokenAlias = "/api/auth/token";
        public const string Refresh = "/api/auth/refresh";
        public const string Logout = "/api/auth/logout";
        public const string Me = "/api/auth/me";
        public const string DeviceBind = "/api/auth/device-bind";
    }

    public static class Client
    {
        public const string Config = "/api/client/config";
        public const string Register = "/api/client/register";
        public const string Heartbeat = "/api/client/heartbeat";
        public const string Capabilities = "/api/client/capabilities";
        public const string Pair = "/api/client/pair";
        public const string LibraryAccess = "/api/client/library-access";
    }

    public static class Discovery
    {
        public const string Server = "/api/discovery/server";
    }

    public static class Memory
    {
        public const string Search = "/api/memory/search";
        public const string Collection = "/api/memory";
        public const string ById = "/api/memory/{id}";
        public const string Projects = "/api/memory/projects";
        public const string Events = "/api/memory/events";
    }

    public static class Agent
    {
        public const string Run = "/api/agent/run";
        public const string Tools = "/api/agent/tools";

        public const string AgentsAlias = "/api/agents";
    }

    public static class WorkOrders
    {
        public const string Collection = "/api/workorders";
        public const string ById = "/api/workorders/{id}";
        public const string Cancel = "/api/workorders/{id}/cancel";
    }

    public static class Duki
    {
        public const string Actions = "/api/duki/actions";
        public const string PendingActions = "/api/duki/actions/pending";
        public const string ApproveAction = "/api/duki/actions/{id}/approve";
        public const string DenyAction = "/api/duki/actions/{id}/deny";
        public const string TransitionAction = "/api/duki/actions/{id}/transition";
        public const string Missions = "/api/duki/missions";
        public const string PlanMission = "/api/duki/missions/plan";
        public const string RunMission = "/api/duki/missions/run";
    }

    public static class Voice
    {
        public const string Transcribe = "/api/voice/transcribe";
        public const string Command = "/api/voice/command";
        public const string Start = "/api/voice/start";
        public const string Stop = "/api/voice/stop";
        public const string Status = "/api/voice/status";
    }

    public static class Comms
    {
        public const string Contacts = "/api/comms/contacts";
        public const string ContactById = "/api/comms/contacts/{id:int}";
        public const string Messages = "/api/comms/messages";
        public const string MessageById = "/api/comms/messages/{id:int}";
        public const string MarkMessageRead = "/api/comms/messages/{id:int}/read";
        public const string Unread = "/api/comms/unread";
        public const string Groups = "/api/comms/groups";
        public const string Tasks = "/api/comms/tasks";
        public const string CommunicateMessageAlias = "/api/communicate/message";
    }

    public static class Providers
    {
        public const string Collection = "/api/providers";
        public const string Ping = "/api/providers/ping";
    }

    public static class Ollama
    {
        public const string Status = "/api/ollama/status";
        public const string Models = "/api/ollama/models";
        public const string Running = "/api/ollama/running";
        public const string ModelByName = "/api/ollama/models/{name}";
        public const string Pull = "/api/ollama/pull";
        public const string DeleteModel = "/api/ollama/models/{name}";
    }

    public static class Extensions
    {
        public const string Catalog          = "/api/extensions/catalog";
        public const string Installed        = "/api/extensions/installed";
        public const string Loaded           = "/api/extensions/loaded";
        public const string ValidateManifest = "/api/extensions/manifest/validate";
        public const string Install          = "/api/extensions/install";
        public const string InstallFromPath  = "/api/extensions/install-from-path";
        public const string Enable           = "/api/extensions/{id}/enable";
        public const string Disable          = "/api/extensions/{id}/disable";
        public const string Uninstall        = "/api/extensions/{id}";
    }

    public static class Push
    {
        public const string Register = "/api/push/register";
        public const string Unregister = "/api/push/unregister";
    }

    public static class Emergency
    {
        public const string DukiPause = "/api/emergency/duki/pause";
        public const string DukiResume = "/api/emergency/duki/resume";
        public const string StopAllAgents = "/api/emergency/agents/stop-all";
        public const string EnableReadOnly = "/api/emergency/readonly/enable";
        public const string DisableReadOnly = "/api/emergency/readonly/disable";
        public const string Status = "/api/emergency/status";
    }

    public static class Bugsack
    {
        public const string Report = "/api/bugsack/report";
        public const string KnownIssues = "/api/bugsack/known-issues";
        public const string Feedback = "/api/bugsack/feedback";
    }

    public static class Update
    {
        public const string Channel = "/api/update/channel";
        public const string Check = "/api/update/check";
        public const string Manifest = "/api/update/manifest";
    }

    public static class Licensing
    {
        public const string Status = "/api/license/status";
        public const string Activate = "/api/license/activate";
        public const string Deactivate = "/api/license/deactivate";
    }

    // ── Routes not yet backed by AaiaApiRoutes constants (added to prevent drift) ──

    public static class Core
    {
        public const string Status      = "/api/core/status";
        public const string Projects    = "/api/core/projects";
        public const string ProjectBySlug = "/api/core/projects/{slug}";
        public const string Knowledge   = "/api/core/knowledge";
        public const string KnowledgeById = "/api/core/knowledge/{id}";
        public const string Decisions   = "/api/core/decisions";
        public const string Solutions   = "/api/core/solutions";
        public const string Errors      = "/api/core/errors";
        public const string Inbox       = "/api/core/inbox";
        public const string InboxById   = "/api/core/inbox/{id}";
        public const string Search      = "/api/core/search";
        public const string Context     = "/api/core/context/{project}";
        public const string CognitiveContext = "/api/core/cognitive-context";
    }

    public static class Settings
    {
        public const string Collection = "/api/settings";
    }

    public static class Users
    {
        public const string Collection = "/api/users";
        public const string ById       = "/api/users/{id}";
    }

    public static class Logs
    {
        public const string Collection = "/api/logs";
    }

    public static class Plugins
    {
        public const string Collection = "/api/plugins";
        public const string Scan       = "/api/plugins/scan";
    }

    public static class Devices
    {
        public const string Register   = "/api/devices/register";
        public const string Collection = "/api/devices";
        public const string Trust      = "/api/devices/{id}/trust";
        public const string Revoke     = "/api/devices/{id}/revoke";
    }

    public static class Security
    {
        public const string SignedWhitelist   = "/api/security/whitelist/signed";
        public const string AgentWhitelist    = "/api/security/whitelist/{agentId}";
    }

    // ── Marketplace API (aaia-marketplace-api — zentraler Webserver) ──────────

    /// <summary>
    /// Developer-Identity-Endpunkte der AAIA Marketplace API.
    /// ETW-IDs leben ausschließlich hier — NIEMALS in einer lokalen AAIAS-DB.
    /// </summary>
    public static class Developers
    {
        public const string Register    = "/api/developers/register";
        public const string Login       = "/api/developers/login";
        public const string VerifyTotp  = "/api/developers/verify-totp";
        /// <summary>Profil des aktuell eingeloggten Entwicklers (JWT erforderlich).</summary>
        public const string Me          = "/api/developers/me";
        public const string GetById     = "/api/developers/{etwId}";
        public const string GetModules  = "/api/developers/{etwId}/modules";
        /// <summary>Löscht einen Pending-Account (kein JWT — nur für Registrierungsabbruch).</summary>
        public const string Delete      = "/api/developers/{etwId}/delete";
        /// <summary>Admin-Route: Öffentlichen Schlüssel eines Entwicklers hinterlegen.</summary>
        public const string RegisterKey = "/api/admin/publisher-keys";

        // ── Phase 5.9: Developer Marketplace Dashboard ────────────────────────

        /// <summary>
        /// GET — Übersicht aller eigenen Extensions inkl. Lizenz- und Verkaufsstatistiken.
        /// JWT (Developer/Partner/Owner) erforderlich.
        /// Gibt <see cref="AAIA.Shared.Contracts.Marketplace.DeveloperDashboardDto"/> zurück.
        /// </summary>
        public const string Dashboard = "/api/developer/dashboard";

        /// <summary>
        /// GET — Alle Extensions des authentifizierten Entwicklers.
        /// Gibt IEnumerable&lt;DeveloperExtensionSummaryDto&gt; zurück.
        /// </summary>
        public const string MyExtensions = "/api/developer/extensions";

        /// <summary>
        /// GET — Verkaufsstatistiken einer Extension (aus MoR-Webhooks).
        /// Geschätzte Werte — kein Ersatz für MoR-Abrechnung.
        /// </summary>
        public const string ExtensionSalesSummary = "/api/developer/extensions/{extensionId}/sales-summary";

        /// <summary>
        /// GET — Lizenzliste einer Extension (Käufer-Email anonymisiert, Statuszähler).
        /// Paginiert: ?page=1&amp;pageSize=50.
        /// </summary>
        public const string ExtensionLicenses = "/api/developer/extensions/{extensionId}/licenses";

        /// <summary>
        /// GET — Letzte MoR-Webhook-Events für eine Extension (max. 50).
        /// Für Debugging von Zahlungsflüssen und fehlenden Lizenzen.
        /// </summary>
        public const string ExtensionWebhookEvents = "/api/developer/extensions/{extensionId}/webhook-events";
    }

    /// <summary>
    /// Marketplace-Endpunkte: Modullisting, Publish, Lizenz-Checks.
    /// Basis-Feed-Routen sind bereits in Extensions definiert —
    /// diese Klasse ergänzt die erweiterten Marketplace-spezifischen Routen.
    /// </summary>
    public static class Marketplace
    {
        public const string Feed         = "/api/marketplace/feed";
        public const string Modules      = "/api/marketplace/modules";
        public const string ModuleById   = "/api/marketplace/modules/{id}";
        public const string Publish      = "/api/marketplace/modules/{id}/publish";
        public const string Developer    = "/api/marketplace/developers/{etwId}";
        /// <summary>Alle Lizenzen des authentifizierten Käufers.</summary>
        public const string MyLicenses   = "/api/marketplace/licenses";
        /// <summary>Admin: Lizenz manuell zuweisen (Freikopien, Support-Kulanz).</summary>
        public const string GrantLicense = "/api/marketplace/licenses/grant";
        /// <summary>Prüft ob ein Käufer (Email) ein Modul lizenziert hat.</summary>
        public const string LicenseCheck = "/api/marketplace/licenses/check";
        /// <summary>Stripe Webhook — Eingang nach Kauf.</summary>
        public const string StripeWebhook = "/api/marketplace/webhooks/stripe";

        /// <summary>
        /// POST — Erstellt eine Stripe Checkout-Session für ein Modul.
        /// Body: <see cref="AAIA.Shared.Contracts.Marketplace.CreateCheckoutSessionRequest"/>.
        /// Gibt <see cref="AAIA.Shared.Contracts.Marketplace.CheckoutSessionDto"/> zurück.
        /// </summary>
        public const string CreateCheckoutSession = "/api/marketplace/checkout";

        // ── Checkout / Pricing (Phase 5.7a) ──────────────────────────────────

        /// <summary>
        /// GET — Checkout-Informationen zu einer Extension: Preis, CheckoutUrl, MoR-Provider.
        /// Kein Auth erforderlich — öffentlich.
        /// WooCommerce und Module Manager nutzen diesen Endpunkt für den "Kaufen"-Button.
        /// </summary>
        public const string ExtensionCheckout = "/api/marketplace/extensions/{extensionId}/checkout";

        // ── License Status (Phase 5.7b) ───────────────────────────────────────

        /// <summary>
        /// GET — Lizenzstatus des authentifizierten Käufers für eine Extension.
        /// Bearer JWT (email-Claim) erforderlich.
        /// Antwort: HasLicense, Status, ExpiresAt, LicenseModel, CanDownload, CheckoutUrl.
        /// Module Manager nutzt diesen Endpunkt für den "Lizenz prüfen"-Button.
        /// </summary>
        public const string ExtensionLicenseStatus = "/api/marketplace/extensions/{extensionId}/license-status";

        // ── License-Token ────────────────────────────────────────────────────

        /// <summary>
        /// POST — Aktiviert eine WooCommerce-Lizenz im Module Manager.
        /// Kein JWT erforderlich — LicenseKey + BuyerEmail sind das Credential.
        /// Rate-Limit: 5 req/min/IP.
        /// Gibt LicenseJwt (Module/Plugin) oder DownloadUrl (LanguagePack) zurück.
        /// </summary>
        public const string ActivateLicense = "/api/marketplace/licenses/activate";

        /// <summary>
        /// Gibt ein signiertes RS256-JWT für eine aktive Lizenz aus.
        /// Token ist an ETW-ID + DeviceId + ModuleId gebunden.
        /// AAIAS verifiziert das Token lokal ohne API-Call.
        /// </summary>
        public const string IssueLicenseToken = "/api/marketplace/licenses/token";

        /// <summary>
        /// Validiert ein Lizenz-Token ohne neues auszustellen.
        /// Für AAIAS: prüft ob Token noch nicht revoked wurde.
        /// </summary>
        public const string ValidateLicenseToken = "/api/marketplace/licenses/token/validate";

        // ── WooCommerce Bridge ───────────────────────────────────────────────
        /// <summary>
        /// POST — WooCommerce-Plugin ruft das nach Bestellabschluss auf.
        /// Auth: X-Bridge-Key Header (kein JWT, Server-zu-Server).
        /// Erstellt Lizenz in der Marketplace-DB und gibt LicenseKey + optional JWT zurück.
        /// </summary>
        public const string WooConfirmOrder = "/api/marketplace/orders/woocommerce/confirm";

        /// <summary>
        /// POST — WooCommerce-Plugin prüft ob ein Lizenz-Schlüssel noch aktiv ist.
        /// Auth: X-Bridge-Key Header.
        /// Fallback-Quelle für wp_aaia_licenses-Cache-Invalidierung.
        /// </summary>
        public const string WooVerifyLicense = "/api/marketplace/orders/woocommerce/verify";

        /// <summary>
        /// GET — Öffentlicher RSA-Schlüssel (PEM) für die Offline-JWT-Verifikation in AAIAS.
        /// Kein Auth erforderlich — der Key ist öffentlich.
        /// AAIAS cached diesen Key (24h) und verifiziert Lizenz-Tokens damit lokal.
        /// </summary>
        public const string PublicKey = "/api/marketplace/public-key";

        // ── Registry (Phase 5.2) ─────────────────────────────────────────────────

        /// <summary>GET — Öffentliche Extension-Liste (nur IsPublished=true).</summary>
        public const string RegistryList     = "/api/marketplace/extensions";

        /// <summary>GET — Details zu einer Extension (neueste Published Release).</summary>
        public const string RegistryById     = "/api/marketplace/extensions/{extensionId}";

        /// <summary>GET — Alle Releases einer Extension (nur Published, nach Version sortiert).</summary>
        public const string RegistryReleases = "/api/marketplace/extensions/{extensionId}/releases";

        /// <summary>GET — Details zu einem bestimmten Release.</summary>
        public const string RegistryRelease  = "/api/marketplace/extensions/{extensionId}/releases/{version}";

        /// <summary>
        /// POST — Verifiziertes Release öffentlich schalten (MarketplaceVerified → IsPublished=true).
        /// JWT (Developer, derselbe wie Publisher) erforderlich.
        /// </summary>
        public const string PublishRelease   = "/api/marketplace/extensions/{extensionId}/releases/{version}/publish";

        // ── Download (Phase 5.3) ──────────────────────────────────────────────

        /// <summary>
        /// GET — Lädt das .aaiaext-Paket eines veröffentlichten Releases herunter.
        /// Anonym zugänglich (kein JWT erforderlich für freie Extensions).
        /// Antwort: application/octet-stream mit SHA256- und Fingerprint-Header.
        /// Header: X-Package-Sha256, X-Key-Fingerprint, X-Signature-Version, X-Trust-Level.
        /// </summary>
        public const string DownloadRelease = "/api/marketplace/extensions/{extensionId}/releases/{version}/download";

        // ── Signed Upload (Phase 5.1) ─────────────────────────────────────────

        /// <summary>
        /// POST multipart/form-data — ETW-signiertes Extension-Paket hochladen.
        /// JWT (Developer) erforderlich. Synchrone Verifikation im Request.
        /// Felder: extensionId, version, developerEtwId, keyFingerprint, trustLevel, signatureVersion,
        ///         package (.aaiaext), signatureInfo (signature-info.json),
        ///         [releaseInfo], [inspectionReport].
        /// Gibt <see cref="AAIA.Shared.Contracts.Marketplace.SignedUploadResponse"/> zurück.
        /// </summary>
        public const string UploadSigned = "/api/marketplace/extensions/upload-signed";

        // ── Phase 5.11: MoR Status ────────────────────────────────────────────
        /// <summary>
        /// GET — MoR-Verbindungsstatus des aktuellen Entwicklers.
        /// Gibt an ob Provider verbunden, Webhook gesund und Checkout aktiv ist.
        /// Enthält keine Bank- oder Steuerdaten.
        /// </summary>
        public const string MorStatus   = "/api/developer/mor/status";

        /// <summary>
        /// PUT — CheckoutUrl einer Extension aktualisieren.
        /// Body: MorProviderUpdateRequest (extensionId + checkoutUrl).
        /// JWT (Developer, Owner des Moduls) erforderlich.
        /// </summary>
        public const string MorProvider = "/api/developer/mor/provider";
    }

    // ── Käuferkonto / Buyer Account (Phase 5.8) ───────────────────────────────

    /// <summary>
    /// Endpunkte für Käuferkonten (kein ETW-Workflow, eigener JWT mit Role="User").
    /// Käufer registrieren sich auf der Webseite oder per E-Mail-Link.
    /// </summary>
    public static class BuyerAccount
    {
        /// <summary>POST — Neues Käuferkonto anlegen. Body: BuyerRegisterRequest.</summary>
        public const string Register = "/api/account/register";

        /// <summary>POST — Login. Body: BuyerLoginRequest. Gibt BuyerTokenResponse zurück.</summary>
        public const string Login    = "/api/account/login";

        // ── Lizenzen ──────────────────────────────────────────────────────────

        /// <summary>
        /// POST — Einmal-Claim-Token einlösen und Lizenz dem Käuferkonto zuordnen.
        /// Body: BuyerClaimLicenseRequest (token).
        /// Käufer-JWT erforderlich (Role = "User").
        /// Rate-Limit: 10 req/min — verhindert Brute-Force auf Claim-Token.
        /// </summary>
        public const string ClaimLicense  = "/api/account/licenses/claim";

        /// <summary>
        /// GET — Alle Lizenzen des authentifizierten Käufers.
        /// Käufer-JWT erforderlich. Gibt IEnumerable&lt;BuyerLicenseDto&gt; zurück.
        /// </summary>
        public const string ListLicenses  = "/api/account/licenses";

        /// <summary>
        /// GET — Details zu einer spezifischen Lizenz des Käufers.
        /// Käufer-JWT erforderlich. 404 wenn Lizenz einem anderen Käufer gehört.
        /// </summary>
        public const string LicenseById   = "/api/account/licenses/{licenseId}";

        /// <summary>
        /// POST — Neuen Claim-Token anfordern (falls abgelaufen oder Mail verloren).
        /// Body: ResendClaimRequest (licenseKey + buyerEmail).
        /// Rate-Limit: 3 req/10min.
        /// </summary>
        public const string ResendClaim   = "/api/account/licenses/resend-claim";
    }

    // ── Admin API (Phase 5.6) ─────────────────────────────────────────────────

    /// <summary>
    /// Admin-Endpunkte für die Verwaltung von MoR-Mappings, Webhook-Events und Lizenzen.
    /// Nur Accounts mit Role = "Owner" haben Zugriff.
    /// </summary>
    public static class Admin
    {
        // MoR Product Mappings
        /// <summary>GET (list, filter) + POST (create) für MorProductMapping.</summary>
        public const string MorMappings     = "/api/admin/mor/mappings";
        /// <summary>PUT (update) + DELETE (soft/hard) für einzelnes Mapping.</summary>
        public const string MorMappingById  = "/api/admin/mor/mappings/{id:int}";

        // Webhook-Event-Log
        /// <summary>GET — Paginierter Log aller MoR-Webhook-Events (inkl. Filter).</summary>
        public const string MorEvents       = "/api/admin/mor/events";
        /// <summary>GET — Einzelnes Event inkl. RawPayload.</summary>
        public const string MorEventById    = "/api/admin/mor/events/{id:int}";

        // Lizenz-Ablauf-Job
        /// <summary>POST — Manueller Trigger: setzt abgelaufene Lizenzen auf Expired.</summary>
        public const string ExpireDueLicenses = "/api/admin/licenses/expire-due";

        // Extension-Pricing (Phase 5.7a)
        /// <summary>PUT — Preis, Währung und CheckoutUrl einer Extension setzen. Owner only.</summary>
        public const string SetExtensionPricing = "/api/admin/extensions/{extensionId}/pricing";

        // Publisher-Keys (bereits vorhanden, hier für Vollständigkeit)
        /// <summary>POST — Öffentlichen ETW-Schlüssel eines Entwicklers hinterlegen.</summary>
        public const string RegisterPublisherKey = "/api/admin/publisher-keys";

        // ── Phase 5.10: Marketplace Console (Owner/Admin) ──────────────────────
        /// <summary>GET — Aggregierte Plattformübersicht (Counts, Webhook-Stats).</summary>
        public const string MarketplaceOverview   = "/api/admin/marketplace/overview";
        /// <summary>GET — Paginierte Extensions-Liste mit Risk-Ampel.</summary>
        public const string AdminExtensions       = "/api/admin/marketplace/extensions";
        /// <summary>GET — Detailansicht einer einzelnen Extension.</summary>
        public const string AdminExtensionById    = "/api/admin/marketplace/extensions/{extensionId}";
        /// <summary>GET — Paginierte Entwickler-Liste.</summary>
        public const string AdminDevelopers       = "/api/admin/marketplace/developers";
        /// <summary>GET — Entwickler-Detailansicht mit Extension-Liste.</summary>
        public const string AdminDeveloperByEtwId = "/api/admin/marketplace/developers/{etwId}";
        /// <summary>GET — Paginierte Lizenz-Liste (alle Kaeufer, filterbar).</summary>
        public const string AdminLicenses         = "/api/admin/marketplace/licenses";
        /// <summary>GET — Releases im Status PendingReview.</summary>
        public const string PendingReviews        = "/api/admin/marketplace/releases/pending-review";
        /// <summary>POST — Release sperren (Owner only).</summary>
        public const string BlockRelease          = "/api/admin/marketplace/releases/{releaseId:int}/block";
        /// <summary>POST — Release-Sperre aufheben (Owner only).</summary>
        public const string UnblockRelease        = "/api/admin/marketplace/releases/{releaseId:int}/unblock";

        // ── Phase 5.11: MoR Account Status ───────────────────────────────────
        /// <summary>
        /// GET — MoR-Verbindungsstatus aller ETW-Konten.
        /// Zeigt: ohne Mapping, ohne CheckoutUrl, Webhook-Probleme, Provider-Mismatch.
        /// Nur Owner/Admin.
        /// </summary>
        public const string MorAccountStatus = "/api/admin/mor/account-status";
    }

    // ── MoR Webhooks (Phase 5.5) ──────────────────────────────────────────────

    /// <summary>
    /// Webhook-Endpoints für Merchant of Record Anbieter.
    /// Beide Endpoints sind öffentlich (kein JWT) — Authentifizierung via HMAC-Signatur.
    /// </summary>
    public static class Mor
    {
        /// <summary>POST — Lemon Squeezy Webhook. Header: X-Signature (HMAC-SHA256).</summary>
        public const string LemonSqueezyWebhook = "/api/mor/lemonsqueezy/webhook";

        /// <summary>POST — Paddle Billing v2 Webhook. Header: Paddle-Signature (ts=...;h1=...).</summary>
        public const string PaddleWebhook        = "/api/mor/paddle/webhook";
    }

    /// <summary>
    /// Revocation-List API.
    /// AAIAS pollt diese regelmäßig und cached das Ergebnis lokal (24h).
    /// Offline-Betrieb: AAIAS verwendet den Cache solange er gültig ist.
    /// </summary>
    public static class Revocations
    {
        /// <summary>Aktuelle vollständige Revocation-Liste (signiert, gecacht).</summary>
        public const string List = "/api/revocations";

        /// <summary>Nur Einträge neuer als ?since=&lt;sequenceNumber&gt;. Für Delta-Updates.</summary>
        public const string Delta = "/api/revocations/delta";

        /// <summary>Prüft ob ein spezifisches Modul/ETW/Key revoked ist.</summary>
        public const string Check = "/api/revocations/check";

        /// <summary>Admin: Neuen Revocation-Eintrag anlegen.</summary>
        public const string Revoke = "/api/revocations";
    }
}
