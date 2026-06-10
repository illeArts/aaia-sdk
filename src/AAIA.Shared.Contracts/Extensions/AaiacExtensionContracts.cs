using System.Text.Json.Serialization;

namespace AAIA.Shared.Contracts.Extensions;

// ─────────────────────────────────────────────────────────────────────────────
// AAIAC Client-Side Extension — Shared DTOs
//
// Only types that need to cross the wire (server ↔ client) or be understood
// by both assemblies live here. All runtime interfaces (IAaiacPlugin,
// IAaiacPluginHost, IAaiacDukiGate, …) live in AAIA.Client.Core.Plugins so
// they can reference IAaiaServerClient without creating a circular dependency.
// ─────────────────────────────────────────────────────────────────────────────

/// <summary>
/// Lifecycle state of a loaded AAIAC plugin.
/// Serialised as string in API responses and local manifest store.
/// </summary>
public enum AaiacPluginState
{
    /// <summary>Package discovered on disk, assembly not yet loaded.</summary>
    Registered,

    /// <summary>Assembly loaded, InitialiseAsync complete, ActivateAsync running.</summary>
    Active,

    /// <summary>Active but voluntarily paused (background tasks sleeping).</summary>
    Suspended,

    /// <summary>StopAsync completed. Can re-activate without process restart.</summary>
    Stopped,

    /// <summary>Last lifecycle call threw — see LastError.</summary>
    Faulted
}

/// <summary>
/// Serialisable snapshot of one AAIAC plugin.
/// Returned by the client plugin-manager list endpoint and reported to the
/// server admin panel so admins can see what is installed on each client.
/// </summary>
public sealed record AaiacPluginInfoDto(
    [property: JsonPropertyName("id")]          string Id,
    [property: JsonPropertyName("displayName")] string DisplayName,
    [property: JsonPropertyName("version")]     string Version,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("state")]       AaiacPluginState State,
    [property: JsonPropertyName("trustStatus")] string TrustStatus,
    [property: JsonPropertyName("packagePath")] string PackagePath,
    [property: JsonPropertyName("lastError")]   string? LastError);

/// <summary>
/// Payload for a client-side DUKI approval request submitted by an AAIAC plugin.
/// Sent to the server (or handled locally) to obtain user approval before
/// executing a sensitive action.
/// </summary>
public sealed record AaiacDukiRequest(
    /// <summary>Plugin-scoped action key (e.g. "delete-local-file").</summary>
    [property: JsonPropertyName("actionKey")]   string ActionKey,

    /// <summary>Human-readable description shown in the approval dialog.</summary>
    [property: JsonPropertyName("description")] string Description,

    /// <summary>Risk level — drives the approval UI colour and audit severity.</summary>
    [property: JsonPropertyName("riskLevel")]   AaiaExtensionRiskLevel RiskLevel,

    /// <summary>Optional key/value pairs shown in the approval dialog detail view.</summary>
    [property: JsonPropertyName("parameters")]  IReadOnlyDictionary<string, string>? Parameters = null);

/// <summary>Thrown when the user explicitly denies a plugin DUKI request.</summary>
public sealed class AaiacDukiDeniedException : Exception
{
    public string ActionId { get; }
    public AaiacDukiDeniedException(string actionId)
        : base($"DUKI action '{actionId}' was denied by the user.") => ActionId = actionId;
}

/// <summary>Thrown when a plugin DUKI request times out without a user response.</summary>
public sealed class AaiacDukiTimeoutException : Exception
{
    public string ActionId { get; }
    public AaiacDukiTimeoutException(string actionId)
        : base($"DUKI action '{actionId}' timed out without a user response.") => ActionId = actionId;
}
