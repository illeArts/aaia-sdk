namespace AAIA.Shared.Contracts.Routes;

public static class CanonicalApiRouteCatalog
{
    public static IReadOnlyList<ApiRouteDescriptor> All { get; } =
    [
        new("GET", AaiaApiRoutes.Health, "Health", "Health"),

        new("GET", AaiaApiRoutes.Server.Info, "Server", "Info"),
        new("GET", AaiaApiRoutes.Server.Status, "Server", "Status"),
        new("GET", AaiaApiRoutes.Server.Capabilities, "Server", "Capabilities"),
        new("GET", AaiaApiRoutes.Server.Version, "Server", "Version"),
        new("GET", AaiaApiRoutes.Server.Readiness, "Server", "Readiness"),
        new("POST", AaiaApiRoutes.Server.EvaluateModels, "Server", "EvaluateModels"),

        new("GET", AaiaApiRoutes.Bootstrap.Status, "Bootstrap", "Status"),
        new("GET", AaiaApiRoutes.Bootstrap.Detect, "Bootstrap", "Detect"),
        new("POST", AaiaApiRoutes.Bootstrap.Setup, "Bootstrap", "Setup"),
        new("POST", AaiaApiRoutes.Bootstrap.Admin, "Bootstrap", "Admin"),

        new("GET", AaiaApiRoutes.Setup.Readiness, "Setup", "Readiness"),
        new("GET", AaiaApiRoutes.Setup.RepairPlan, "Setup", "RepairPlan"),
        new("POST", AaiaApiRoutes.Setup.Preflight, "Setup", "Preflight"),
        new("POST", AaiaApiRoutes.Setup.Repair, "Setup", "Repair"),

        new("POST", AaiaApiRoutes.Auth.Login, "Auth", "Login"),
        new("POST", AaiaApiRoutes.Auth.TokenAlias, "Auth", "TokenAlias", false, "iOS existing /api/auth/token"),
        new("POST", AaiaApiRoutes.Auth.Refresh, "Auth", "Refresh"),
        new("POST", AaiaApiRoutes.Auth.Logout, "Auth", "Logout"),
        new("GET", AaiaApiRoutes.Auth.Me, "Auth", "Me"),
        new("POST", AaiaApiRoutes.Auth.DeviceBind, "Auth", "DeviceBind"),

        new("GET", AaiaApiRoutes.Client.Config, "Client", "Config"),
        new("POST", AaiaApiRoutes.Client.Register, "Client", "Register"),
        new("POST", AaiaApiRoutes.Client.Heartbeat, "Client", "Heartbeat"),
        new("GET", AaiaApiRoutes.Client.Capabilities, "Client", "Capabilities"),
        new("POST", AaiaApiRoutes.Client.Pair, "Client", "Pair"),
        new("GET", AaiaApiRoutes.Client.LibraryAccess, "Client", "LibraryAccess"),

        new("GET", AaiaApiRoutes.Discovery.Server, "Discovery", "Server"),

        new("GET", AaiaApiRoutes.Memory.Search, "Memory", "Search"),
        new("GET", AaiaApiRoutes.Memory.ById, "Memory", "GetById"),
        new("POST", AaiaApiRoutes.Memory.Collection, "Memory", "Create"),
        new("PUT", AaiaApiRoutes.Memory.ById, "Memory", "Update"),
        new("DELETE", AaiaApiRoutes.Memory.ById, "Memory", "Delete"),
        new("GET", AaiaApiRoutes.Memory.Projects, "Memory", "Projects", false, "Windows Client current route"),
        new("POST", AaiaApiRoutes.Memory.Events, "Memory", "Events", false, "Windows Client current route"),

        new("POST", AaiaApiRoutes.Agent.Run, "Agent", "Run"),
        new("GET", AaiaApiRoutes.Agent.Tools, "Agent", "Tools"),
        new("GET", AaiaApiRoutes.Agent.AgentsAlias, "Agent", "AgentsAlias", false, "iOS existing /api/agents"),

        new("GET", AaiaApiRoutes.WorkOrders.Collection, "WorkOrders", "List"),
        new("GET", AaiaApiRoutes.WorkOrders.ById, "WorkOrders", "GetById"),
        new("POST", AaiaApiRoutes.WorkOrders.Collection, "WorkOrders", "Create"),
        new("POST", AaiaApiRoutes.WorkOrders.Cancel, "WorkOrders", "Cancel"),

        new("GET", AaiaApiRoutes.Duki.Actions, "DUKI", "Actions"),
        new("GET", AaiaApiRoutes.Duki.PendingActions, "DUKI", "PendingActions"),
        new("POST", AaiaApiRoutes.Duki.ApproveAction, "DUKI", "ApproveAction"),
        new("POST", AaiaApiRoutes.Duki.DenyAction, "DUKI", "DenyAction"),
        new("POST", AaiaApiRoutes.Duki.TransitionAction, "DUKI", "TransitionAction"),
        new("GET", AaiaApiRoutes.Duki.Missions, "DUKI", "Missions"),
        new("POST", AaiaApiRoutes.Duki.PlanMission, "DUKI", "PlanMission"),
        new("POST", AaiaApiRoutes.Duki.RunMission, "DUKI", "RunMission"),

        new("POST", AaiaApiRoutes.Voice.Transcribe, "Voice", "Transcribe"),
        new("POST", AaiaApiRoutes.Voice.Command, "Voice", "Command"),
        new("POST", AaiaApiRoutes.Voice.Start, "Voice", "Start"),
        new("POST", AaiaApiRoutes.Voice.Stop, "Voice", "Stop"),
        new("GET", AaiaApiRoutes.Voice.Status, "Voice", "Status"),

        new("GET", AaiaApiRoutes.Comms.Contacts, "Comms", "Contacts"),
        new("POST", AaiaApiRoutes.Comms.Contacts, "Comms", "CreateContact"),
        new("GET", AaiaApiRoutes.Comms.Messages, "Comms", "Messages"),
        new("POST", AaiaApiRoutes.Comms.Messages, "Comms", "CreateMessage"),
        new("POST", AaiaApiRoutes.Comms.MarkMessageRead, "Comms", "MarkMessageRead"),
        new("GET", AaiaApiRoutes.Comms.Unread, "Comms", "Unread"),
        new("POST", AaiaApiRoutes.Comms.CommunicateMessageAlias, "Comms", "CommunicateMessageAlias", false, "iOS existing /api/communicate/message"),

        new("GET", AaiaApiRoutes.Providers.Collection, "Providers", "Collection", false, "Windows Client current provider status route"),
        new("GET", AaiaApiRoutes.Providers.Ping, "Providers", "Ping", false, "Windows Client current Ollama/provider ping route"),

        new("POST", AaiaApiRoutes.Devices.Register, "Devices", "Register", false, "Client device self-registration"),
        new("GET", AaiaApiRoutes.Devices.Collection, "Devices", "Collection"),
        new("POST", AaiaApiRoutes.Devices.Trust, "Devices", "Trust"),
        new("POST", AaiaApiRoutes.Devices.Revoke, "Devices", "Revoke"),
        new("GET", AaiaApiRoutes.Security.SignedWhitelist, "Security", "SignedWhitelist"),
        new("GET", AaiaApiRoutes.Security.AgentWhitelist, "Security", "AgentWhitelist"),

        new("GET", AaiaApiRoutes.Core.Status, "Core", "Status"),
        new("GET", AaiaApiRoutes.Core.Projects, "Core", "Projects"),
        new("GET", AaiaApiRoutes.Core.ProjectBySlug, "Core", "ProjectBySlug"),
        new("GET", AaiaApiRoutes.Core.Knowledge, "Core", "Knowledge"),
        new("POST", AaiaApiRoutes.Core.Knowledge, "Core", "CreateKnowledge"),
        new("PUT", AaiaApiRoutes.Core.KnowledgeById, "Core", "UpdateKnowledge"),
        new("DELETE", AaiaApiRoutes.Core.KnowledgeById, "Core", "DeleteKnowledge"),
        new("GET", AaiaApiRoutes.Core.Decisions, "Core", "Decisions"),
        new("GET", AaiaApiRoutes.Core.Solutions, "Core", "Solutions"),
        new("GET", AaiaApiRoutes.Core.Errors, "Core", "Errors"),
        new("GET", AaiaApiRoutes.Core.Inbox, "Core", "Inbox"),
        new("POST", AaiaApiRoutes.Core.Inbox, "Core", "AddInbox"),
        new("PATCH", AaiaApiRoutes.Core.InboxById, "Core", "SetInboxStatus"),
        new("GET", AaiaApiRoutes.Core.Search, "Core", "Search"),
        new("GET", AaiaApiRoutes.Core.Context, "Core", "Context"),
        new("GET", AaiaApiRoutes.Core.CognitiveContext, "Core", "CognitiveContext"),

        new("GET", AaiaApiRoutes.Ollama.Status, "Ollama", "Status"),
        new("GET", AaiaApiRoutes.Ollama.Models, "Ollama", "Models"),
        new("GET", AaiaApiRoutes.Ollama.Running, "Ollama", "Running"),
        new("GET", AaiaApiRoutes.Ollama.ModelByName, "Ollama", "ModelByName"),
        new("POST", AaiaApiRoutes.Ollama.Pull, "Ollama", "Pull"),
        new("DELETE", AaiaApiRoutes.Ollama.DeleteModel, "Ollama", "DeleteModel"),

        new("GET", AaiaApiRoutes.Extensions.Catalog, "Extensions", "Catalog"),
        new("GET", AaiaApiRoutes.Extensions.Installed, "Extensions", "Installed"),
        new("GET", AaiaApiRoutes.Extensions.Loaded, "Extensions", "Loaded"),
        new("POST", AaiaApiRoutes.Extensions.ValidateManifest, "Extensions", "ValidateManifest"),
        new("POST", AaiaApiRoutes.Extensions.InstallFromPath, "Extensions", "InstallFromPath"),

        new("POST", AaiaApiRoutes.Push.Register, "Push", "Register", false, "iOS mobile notification route"),
        new("DELETE", AaiaApiRoutes.Push.Unregister, "Push", "Unregister", false, "iOS mobile notification route"),

        new("POST", AaiaApiRoutes.Emergency.DukiPause, "Emergency", "DukiPause"),
        new("POST", AaiaApiRoutes.Emergency.DukiResume, "Emergency", "DukiResume"),
        new("POST", AaiaApiRoutes.Emergency.StopAllAgents, "Emergency", "StopAllAgents"),
        new("POST", AaiaApiRoutes.Emergency.EnableReadOnly, "Emergency", "EnableReadOnly"),
        new("POST", AaiaApiRoutes.Emergency.DisableReadOnly, "Emergency", "DisableReadOnly"),
        new("GET", AaiaApiRoutes.Emergency.Status, "Emergency", "Status"),

        new("POST", AaiaApiRoutes.Bugsack.Report, "Bugsack", "Report"),
        new("GET", AaiaApiRoutes.Bugsack.KnownIssues, "Bugsack", "KnownIssues"),
        new("POST", AaiaApiRoutes.Bugsack.Feedback, "Bugsack", "Feedback"),

        new("GET", AaiaApiRoutes.Update.Channel, "Update", "Channel"),
        new("GET", AaiaApiRoutes.Update.Check, "Update", "Check"),
        new("GET", AaiaApiRoutes.Update.Manifest, "Update", "Manifest"),

        new("GET", AaiaApiRoutes.Licensing.Status, "Licensing", "Status"),
        new("POST", AaiaApiRoutes.Licensing.Activate, "Licensing", "Activate"),
        new("POST", AaiaApiRoutes.Licensing.Deactivate, "Licensing", "Deactivate"),
    ];
}
