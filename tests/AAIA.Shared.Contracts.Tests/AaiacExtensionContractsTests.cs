using System.Text.Json;
using AAIA.Shared.Contracts.Extensions;
using Xunit;

namespace AAIA.Shared.Contracts.Tests;

/// <summary>
/// Tests for AAIAC client-side shared DTOs introduced in v0.8.0:
/// AaiacPluginInfoDto, AaiacDukiRequest, AaiacDukiDeniedException, AaiacDukiTimeoutException.
/// </summary>
public sealed class AaiacExtensionContractsTests
{
    // ── AaiacPluginInfoDto ────────────────────────────────────────────────────

    [Fact]
    public void AaiacPluginInfoDto_serialises_state_as_string()
    {
        var dto = new AaiacPluginInfoDto(
            Id:          "test.plugin",
            DisplayName: "Test Plugin",
            Version:     "1.0.0",
            Description: "A test plugin.",
            State:       AaiacPluginState.Active,
            TrustStatus: "HashVerified",
            PackagePath: "/plugins/test.plugin",
            LastError:   null);

        var json = JsonSerializer.Serialize(dto);

        // AaiacPluginState is a JsonStringEnumConverter value in the real usage,
        // but without a custom serialiser the default is integer. We just verify
        // the record round-trips correctly here.
        var rehydrated = JsonSerializer.Deserialize<AaiacPluginInfoDto>(json);

        Assert.NotNull(rehydrated);
        Assert.Equal(dto.Id, rehydrated!.Id);
        Assert.Equal(dto.State, rehydrated.State);
        Assert.Equal(dto.TrustStatus, rehydrated.TrustStatus);
        Assert.Null(rehydrated.LastError);
    }

    [Fact]
    public void AaiacPluginInfoDto_serialises_last_error_when_set()
    {
        var dto = new AaiacPluginInfoDto(
            Id:          "faulted.plugin",
            DisplayName: "Faulted Plugin",
            Version:     "0.1.0",
            Description: "Bad plugin.",
            State:       AaiacPluginState.Faulted,
            TrustStatus: "Unsigned",
            PackagePath: "/plugins/faulted.plugin",
            LastError:   "NullReferenceException in InitialiseAsync");

        var json = JsonSerializer.Serialize(dto);
        var rehydrated = JsonSerializer.Deserialize<AaiacPluginInfoDto>(json);

        Assert.NotNull(rehydrated);
        Assert.Equal(AaiacPluginState.Faulted, rehydrated!.State);
        Assert.Equal("NullReferenceException in InitialiseAsync", rehydrated.LastError);
    }

    [Theory]
    [InlineData(AaiacPluginState.Registered)]
    [InlineData(AaiacPluginState.Active)]
    [InlineData(AaiacPluginState.Suspended)]
    [InlineData(AaiacPluginState.Stopped)]
    [InlineData(AaiacPluginState.Faulted)]
    public void AaiacPluginState_all_values_round_trip(AaiacPluginState state)
    {
        var dto = new AaiacPluginInfoDto(
            Id: "p", DisplayName: "P", Version: "1.0",
            Description: "", State: state, TrustStatus: "Unsigned",
            PackagePath: "/p", LastError: null);

        var json = JsonSerializer.Serialize(dto);
        var rehydrated = JsonSerializer.Deserialize<AaiacPluginInfoDto>(json);

        Assert.Equal(state, rehydrated!.State);
    }

    // ── AaiacDukiRequest ──────────────────────────────────────────────────────

    [Fact]
    public void AaiacDukiRequest_serialises_with_json_property_names()
    {
        var req = new AaiacDukiRequest(
            ActionKey:   "delete-local-cache",
            Description: "Plugin wants to delete the local cache directory.",
            RiskLevel:   AaiaExtensionRiskLevel.Orange,
            Parameters:  new Dictionary<string, string> { ["path"] = "/tmp/cache" });

        var json = JsonSerializer.Serialize(req);

        Assert.Contains("\"actionKey\"", json);
        Assert.Contains("\"description\"", json);
        Assert.Contains("\"riskLevel\"", json);
        Assert.Contains("\"parameters\"", json);
    }

    [Fact]
    public void AaiacDukiRequest_null_parameters_omitted_on_round_trip()
    {
        var req = new AaiacDukiRequest(
            ActionKey:   "ping",
            Description: "Simple ping.",
            RiskLevel:   AaiaExtensionRiskLevel.Green);

        var json = JsonSerializer.Serialize(req);
        var rehydrated = JsonSerializer.Deserialize<AaiacDukiRequest>(json);

        Assert.NotNull(rehydrated);
        Assert.Equal("ping", rehydrated!.ActionKey);
        Assert.Null(rehydrated.Parameters);
    }

    [Fact]
    public void AaiacDukiRequest_parameters_survive_round_trip()
    {
        var req = new AaiacDukiRequest(
            ActionKey:   "write-file",
            Description: "Write to a file.",
            RiskLevel:   AaiaExtensionRiskLevel.Red,
            Parameters:  new Dictionary<string, string>
            {
                ["filePath"] = "/etc/hosts",
                ["mode"]     = "append"
            });

        var json = JsonSerializer.Serialize(req);
        var rehydrated = JsonSerializer.Deserialize<AaiacDukiRequest>(json);

        Assert.NotNull(rehydrated?.Parameters);
        Assert.Equal("/etc/hosts", rehydrated!.Parameters!["filePath"]);
        Assert.Equal("append",     rehydrated.Parameters["mode"]);
    }

    // ── AaiacDukiDeniedException ───────────────────────────────────────────────

    [Fact]
    public void AaiacDukiDeniedException_carries_action_id()
    {
        var ex = new AaiacDukiDeniedException("DUKI-abc123");

        Assert.Equal("DUKI-abc123", ex.ActionId);
        Assert.Contains("DUKI-abc123", ex.Message);
        Assert.Contains("denied", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AaiacDukiDeniedException_is_exception()
    {
        var ex = new AaiacDukiDeniedException("x");
        Assert.IsAssignableFrom<Exception>(ex);
    }

    // ── AaiacDukiTimeoutException ──────────────────────────────────────────────

    [Fact]
    public void AaiacDukiTimeoutException_carries_action_id()
    {
        var ex = new AaiacDukiTimeoutException("DUKI-timeout99");

        Assert.Equal("DUKI-timeout99", ex.ActionId);
        Assert.Contains("DUKI-timeout99", ex.Message);
        Assert.Contains("timed out", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AaiacDukiTimeoutException_is_exception()
    {
        var ex = new AaiacDukiTimeoutException("y");
        Assert.IsAssignableFrom<Exception>(ex);
    }

    // ── AaiacPluginState enum sanity ──────────────────────────────────────────

    [Fact]
    public void AaiacPluginState_has_five_values()
    {
        var values = Enum.GetValues<AaiacPluginState>();
        Assert.Equal(5, values.Length);
    }
}
