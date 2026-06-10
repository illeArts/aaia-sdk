namespace AAIA.Shared.Contracts.Dev;

/// <summary>
/// Central diagnostics bus for Developer Mode.
///
/// Usage pattern:
///   - Subsystems call Publish() to emit events.
///   - Dev-UI components call Subscribe() to receive a real-time stream.
///   - In production (IsDevMode == false) Publish() is a no-op and Subscribe() never fires.
///
/// Thread-safe. Safe to call from any thread.
/// </summary>
public interface IDevDiagnosticsBus
{
    /// <summary>True when Developer Mode is active and events are forwarded to subscribers.</summary>
    bool IsActive { get; }

    /// <summary>
    /// Emit a diagnostics event. No-op when IsActive is false.
    /// The event is dispatched synchronously to all current subscribers.
    /// </summary>
    void Publish(DevDiagnosticsEvent evt);

    /// <summary>
    /// Subscribe to all future diagnostics events.
    /// Returns an IDisposable — call Dispose() to unsubscribe.
    /// The handler is called on the publisher's thread; do not block.
    /// </summary>
    IDisposable Subscribe(Action<DevDiagnosticsEvent> handler);

    /// <summary>
    /// Returns the last N events (ring buffer, newest first).
    /// Returns an empty list when IsActive is false or no events have been published.
    /// </summary>
    IReadOnlyList<DevDiagnosticsEvent> GetRecent(int maxCount = 200);
}

/// <summary>
/// Null-object implementation — published events are discarded.
/// Used in production or when developer mode is disabled.
/// </summary>
public sealed class NullDevDiagnosticsBus : IDevDiagnosticsBus
{
    public static readonly NullDevDiagnosticsBus Instance = new();

    public bool IsActive => false;
    public void Publish(DevDiagnosticsEvent evt) { }
    public IDisposable Subscribe(Action<DevDiagnosticsEvent> handler) => NoopDisposable.Instance;
    public IReadOnlyList<DevDiagnosticsEvent> GetRecent(int maxCount = 200) => [];

    private sealed class NoopDisposable : IDisposable
    {
        public static readonly NoopDisposable Instance = new();
        public void Dispose() { }
    }
}
