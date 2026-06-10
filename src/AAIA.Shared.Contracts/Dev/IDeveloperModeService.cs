namespace AAIA.Shared.Contracts.Dev;

/// <summary>
/// Controls and reports Developer Mode state for an AAIA host (server or client).
///
/// Activation sources (checked in order, highest priority first):
///   1. CLI flag --dev
///   2. Environment variable AAIA_DEV_MODE=1
///   3. appsettings.json → "DeveloperMode": true
///   4. Runtime toggle via Enable() / Disable() (SuperAdmin only on server)
///
/// SECURITY RULE: Dev Mode must never be silently active.
/// When enabled, the host must display a visible DEV banner/badge
/// and write an audit log entry.
/// </summary>
public interface IDeveloperModeService
{
    /// <summary>True when Developer Mode is currently active.</summary>
    bool IsEnabled { get; }

    /// <summary>The source that last activated Dev Mode (e.g. "cli-flag", "env-var", "settings", "runtime").</summary>
    string? ActivationSource { get; }

    /// <summary>Enable Developer Mode at runtime. Triggers Changed event.</summary>
    void Enable(string source = "runtime");

    /// <summary>Disable Developer Mode at runtime. Triggers Changed event.</summary>
    void Disable();

    event EventHandler<DevModeChangedEventArgs> Changed;
}

public sealed class DevModeChangedEventArgs : EventArgs
{
    public bool IsEnabled       { get; init; }
    public string? Source       { get; init; }
    public DateTimeOffset When  { get; init; } = DateTimeOffset.UtcNow;
}
