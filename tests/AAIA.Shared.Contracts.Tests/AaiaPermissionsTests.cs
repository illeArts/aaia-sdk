using AAIA.Shared.Contracts.V3;
using Xunit;

namespace AAIA.Shared.Contracts.Tests;

/// <summary>
/// Contract tests for <see cref="AaiaPermissions"/> — the canonical, stable
/// permission identifier catalog introduced with AAIA.Shared.Contracts 2.2.1.
///
/// These tests are intentionally strict: any change to IDs, count, or
/// contract version must be a deliberate, reviewed action — not a side-effect.
/// </summary>
public sealed class AaiaPermissionsTests
{
    // ── Exact permission IDs (contract v0.7.2) ───────────────────────────────

    private static readonly string[] ExpectedIds =
    [
        "duki.observe",
        "duki.analyze",
        "duki.work",
        "duki.communicate",
        "duki.admin",
        "duki.approve",
        "email.read",
        "email.reply-draft",
        "email.send",
    ];

    [Fact]
    public void ContractVersion_is_0_7_2()
    {
        Assert.Equal("0.7.2", AaiaPermissions.ContractVersion);
    }

    [Fact]
    public void All_contains_exactly_9_canonical_permission_ids()
    {
        Assert.Equal(9, AaiaPermissions.All.Count);
    }

    [Fact]
    public void All_contains_every_expected_permission_with_exact_spelling()
    {
        foreach (var id in ExpectedIds)
            Assert.Contains(id, AaiaPermissions.All);
    }

    [Fact]
    public void All_contains_no_duplicate_entries()
    {
        var unique = new HashSet<string>(AaiaPermissions.All, StringComparer.Ordinal);
        Assert.Equal(AaiaPermissions.All.Count, unique.Count);
    }

    [Fact]
    public void All_is_not_castable_to_a_mutable_collection()
    {
        // IReadOnlySet<string> must not be silently writable.
        // If the underlying type is ever changed to a plain HashSet (exposed
        // as the interface), this cast would succeed and callers could mutate
        // the shared catalog. We verify the public contract stays immutable.
        var set = AaiaPermissions.All;
        Assert.False(set is HashSet<string>,
            "All must not expose a raw HashSet<string> — use FrozenSet or a read-only wrapper.");
    }

    // ── IsKnown — fail-closed semantics ──────────────────────────────────────

    [Theory]
    [InlineData("duki.observe")]
    [InlineData("duki.analyze")]
    [InlineData("duki.work")]
    [InlineData("duki.communicate")]
    [InlineData("duki.admin")]
    [InlineData("duki.approve")]
    [InlineData("email.read")]
    [InlineData("email.reply-draft")]
    [InlineData("email.send")]
    public void IsKnown_returns_true_for_every_canonical_id(string permission)
    {
        Assert.True(AaiaPermissions.IsKnown(permission));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("duki.unknown")]
    [InlineData("email.forward")]
    [InlineData("server.admin")]
    [InlineData("DUKI.OBSERVE")]      // wrong casing
    [InlineData("duki.observe ")]     // trailing space
    public void IsKnown_returns_false_for_null_empty_and_unknown_ids(string? permission)
    {
        Assert.False(AaiaPermissions.IsKnown(permission));
    }

    // ── Nested constant classes ───────────────────────────────────────────────

    [Fact]
    public void Duki_constants_match_All_entries()
    {
        Assert.Contains(AaiaPermissions.Duki.Observe,    AaiaPermissions.All);
        Assert.Contains(AaiaPermissions.Duki.Analyze,    AaiaPermissions.All);
        Assert.Contains(AaiaPermissions.Duki.Work,       AaiaPermissions.All);
        Assert.Contains(AaiaPermissions.Duki.Communicate, AaiaPermissions.All);
        Assert.Contains(AaiaPermissions.Duki.Admin,      AaiaPermissions.All);
        Assert.Contains(AaiaPermissions.Duki.Approve,    AaiaPermissions.All);
    }

    [Fact]
    public void Email_constants_match_All_entries()
    {
        Assert.Contains(AaiaPermissions.Email.Read,       AaiaPermissions.All);
        Assert.Contains(AaiaPermissions.Email.ReplyDraft, AaiaPermissions.All);
        Assert.Contains(AaiaPermissions.Email.Send,       AaiaPermissions.All);
    }
}
