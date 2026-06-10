namespace Aaia.Shared.Contracts.Modules;

/// <summary>
/// Optional interface for server-side modules that support WorkOrder sandbox execution.
///
/// When a module implements this interface, the DevController's /api/dev/workorder/simulate
/// endpoint will dispatch the WorkOrder JSON directly to the module and return the result.
///
/// Modules that do NOT implement this interface will receive an informative "not supported"
/// response from the simulation endpoint.
/// </summary>
public interface IWorkOrderSimulator
{
    /// <summary>
    /// Execute a WorkOrder payload in simulation/sandbox mode.
    ///
    /// The implementation should:
    ///   - Parse workOrderJson into its internal model
    ///   - Execute the work order logic
    ///   - Return a result JSON and any diagnostic info
    ///
    /// Exceptions thrown here are caught by LoadedAaiaModules and surfaced as
    /// WorkOrderSimulationResult with Success=false and the exception message.
    /// </summary>
    Task<WorkOrderSimulationResult> SimulateAsync(
        string workOrderJson,
        CancellationToken ct = default);
}

/// <summary>Result of a WorkOrder sandbox execution.</summary>
public sealed record WorkOrderSimulationResult(
    bool     Success,
    string?  ResultJson,
    string?  ErrorCode,
    string?  ErrorMessage,
    string?  StackTrace,
    TimeSpan Duration)
{
    public static WorkOrderSimulationResult Ok(string? resultJson, TimeSpan duration)
        => new(true, resultJson, null, null, null, duration);

    public static WorkOrderSimulationResult Fail(
        string errorCode,
        string errorMessage,
        string? stackTrace,
        TimeSpan duration)
        => new(false, null, errorCode, errorMessage, stackTrace, duration);
}
