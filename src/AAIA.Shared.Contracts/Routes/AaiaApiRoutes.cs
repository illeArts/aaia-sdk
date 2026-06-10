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
}
