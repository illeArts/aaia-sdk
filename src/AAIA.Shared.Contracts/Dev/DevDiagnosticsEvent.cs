namespace AAIA.Shared.Contracts.Dev;

/// <summary>
/// Structured diagnostics event emitted by any AAIA subsystem when Developer Mode is active.
///
/// Every subsystem (PluginManager, ExtensionLoader, ModuleRegistry, WorkOrderRouter …)
/// publishes to IDevDiagnosticsBus. Dev-UI components subscribe to receive real-time events.
/// Nothing is published in production mode.
/// </summary>
public sealed record DevDiagnosticsEvent
{
    /// <summary>Error code from AaiaErrorCodes (e.g. "AAIA-ASM-0201"). Null for informational events.</summary>
    public string? ErrorCode { get; init; }

    /// <summary>Subsystem that emitted the event: "PluginManager", "ExtensionLoader", "ModuleRegistry" …</summary>
    public required string Source { get; init; }

    /// <summary>Plugin/module/extension id the event relates to. Empty for system-level events.</summary>
    public string ComponentId { get; init; } = string.Empty;

    public required DevEventSeverity Severity { get; init; }

    public required string Message { get; init; }

    /// <summary>Full stack trace if the event was triggered by an exception. Null otherwise.</summary>
    public string? StackTrace { get; init; }

    /// <summary>Inner exception message chain, already formatted as "Type: Message → InnerType: InnerMessage".</summary>
    public string? InnerException { get; init; }

    /// <summary>Hint for the developer — actionable suggestion to fix the problem.</summary>
    public string? Hint { get; init; }

    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;

    /// <summary>Arbitrary structured properties (assembly path, version, manifest field …).</summary>
    public IReadOnlyDictionary<string, object?> Properties { get; init; }
        = new Dictionary<string, object?>();

    // ── Factories ─────────────────────────────────────────────────────────────

    public static DevDiagnosticsEvent Info(string source, string componentId, string message,
        IReadOnlyDictionary<string, object?>? props = null)
        => new()
        {
            Source      = source,
            ComponentId = componentId,
            Severity    = DevEventSeverity.Info,
            Message     = message,
            Properties  = props ?? new Dictionary<string, object?>()
        };

    public static DevDiagnosticsEvent Warning(string source, string componentId, string message,
        string? errorCode = null, string? hint = null,
        IReadOnlyDictionary<string, object?>? props = null)
        => new()
        {
            Source      = source,
            ComponentId = componentId,
            Severity    = DevEventSeverity.Warning,
            ErrorCode   = errorCode,
            Message     = message,
            Hint        = hint,
            Properties  = props ?? new Dictionary<string, object?>()
        };

    public static DevDiagnosticsEvent FromException(string source, string componentId,
        Exception ex, string message, string? errorCode = null, string? hint = null,
        IReadOnlyDictionary<string, object?>? props = null)
        => new()
        {
            Source         = source,
            ComponentId    = componentId,
            Severity       = DevEventSeverity.Error,
            ErrorCode      = errorCode,
            Message        = message,
            StackTrace     = ex.StackTrace,
            InnerException = FormatInnerChain(ex),
            Hint           = hint,
            Properties     = props ?? new Dictionary<string, object?>()
        };

    private static string FormatInnerChain(Exception ex)
    {
        var parts = new List<string>();
        var current = ex;
        while (current is not null)
        {
            parts.Add($"{current.GetType().Name}: {current.Message}");
            current = current.InnerException;
        }
        return string.Join(" → ", parts);
    }
}
