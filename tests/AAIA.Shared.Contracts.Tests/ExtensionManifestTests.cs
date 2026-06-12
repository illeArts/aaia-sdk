using AAIA.Shared.Contracts.Extensions;
using Xunit;

namespace AAIA.Shared.Contracts.Tests;

public sealed class ExtensionManifestTests
{
    [Fact]
    public void Aaiac_extension_cannot_request_server_admin_permission()
    {
        var manifest = ValidManifest(
            AaiaExtensionHost.AAIAC,
            [new AaiaExtensionPermissionDto(
                AaiaExtensionPermissionScope.ServerAdmin,
                "Should not be allowed",
                AaiaExtensionRiskLevel.Red,
                RequiresDuki: true,
                RequiresAdminApproval: true)]);

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("ServerAdmin", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Orange_or_red_permissions_must_require_duki()
    {
        var manifest = ValidManifest(
            AaiaExtensionHost.AAIAS,
            [new AaiaExtensionPermissionDto(
                AaiaExtensionPermissionScope.ServerWrite,
                "Needs server write action",
                AaiaExtensionRiskLevel.Orange,
                RequiresDuki: false,
                RequiresAdminApproval: true)]);

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("DUKI", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Aaias_extension_cannot_request_client_local_permissions()
    {
        var manifest = ValidManifest(
            AaiaExtensionHost.AAIAS,
            [new AaiaExtensionPermissionDto(
                AaiaExtensionPermissionScope.ClientLocalAdmin,
                "Local admin belongs to AAIAC.",
                AaiaExtensionRiskLevel.Orange,
                RequiresDuki: true,
                RequiresAdminApproval: true)]);

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("ClientLocal", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("AAIAS.Module.Bad")]
    [InlineData("aa")]
    [InlineData("../bad")]
    public void Id_must_use_safe_lowercase_identifier_format(string id)
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            Id = id
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("id must use", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Duplicate_permission_scopes_are_rejected()
    {
        var manifest = ValidManifest(
            AaiaExtensionHost.AAIAS,
            [ServerReadPermission(), ServerReadPermission()]);

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("duplicate scope", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Orange_or_red_permissions_must_require_admin_approval()
    {
        var manifest = ValidManifest(
            AaiaExtensionHost.AAIAS,
            [new AaiaExtensionPermissionDto(
                AaiaExtensionPermissionScope.ServerWrite,
                "Needs server write action",
                AaiaExtensionRiskLevel.Orange,
                RequiresDuki: true,
                RequiresAdminApproval: false)]);

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("admin approval", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Secret_access_requires_declared_secret_names()
    {
        var manifest = ValidManifest(
            AaiaExtensionHost.AAIAS,
            [new AaiaExtensionPermissionDto(
                AaiaExtensionPermissionScope.SecretAccess,
                "Read configured secret.",
                AaiaExtensionRiskLevel.Orange,
                RequiresDuki: true,
                RequiresAdminApproval: true)]);

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("SecretAccess", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Network_access_requires_declared_http_targets()
    {
        var manifest = ValidManifest(
            AaiaExtensionHost.AAIAS,
            [new AaiaExtensionPermissionDto(
                AaiaExtensionPermissionScope.NetworkAccess,
                "Call local service.",
                AaiaExtensionRiskLevel.Yellow,
                RequiresDuki: false,
                RequiresAdminApproval: true)]);

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("NetworkAccess", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Duplicate_routes_are_rejected()
    {
        var route = new AaiaExtensionRouteDto(
            "GET",
            "/api/modules/test/status",
            AaiaExtensionRiskLevel.Green,
            RequiresAuth: true,
            RequiresAdmin: false,
            RequiresDuki: false);
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            Routes = [route, route]
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("duplicate route", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Orange_mutating_routes_must_require_duki()
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            Routes =
            [
                new AaiaExtensionRouteDto(
                    "POST",
                    "/api/modules/test/reboot",
                    AaiaExtensionRiskLevel.Orange,
                    RequiresAuth: true,
                    RequiresAdmin: true,
                    RequiresDuki: false)
            ]
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("must require DUKI", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Signing_hash_must_be_sha256_hex()
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            Signing = new AaiaExtensionSigningDto(
                PublisherKeyId: "aaia.tests",
                PackageSha256: "not-a-hash",
                Signature: "placeholder",
                SignatureAlgorithm: "sha256-dev-placeholder")
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("packageSha256", StringComparison.OrdinalIgnoreCase));
    }

    private static AaiaExtensionManifestDto ValidManifest(
        AaiaExtensionHost host,
        IReadOnlyList<AaiaExtensionPermissionDto> permissions) =>
        new(
            Id: "test.extension",
            DisplayName: "Test Extension",
            Version: "1.0.0",
            Host: host,
            Kind: AaiaExtensionKind.Plugin,
            PublisherId: "test-publisher",
            Description: "Test extension",
            Permissions: permissions,
            Routes: [],
            RequiredSecrets: [],
            NetworkTargets: [],
            SupportedPlatforms: ["windows"],
            Signing: null);

    // ── Marketplace / Publisher-Validierung (v2.1) ──────────────────────────

    [Theory]
    [InlineData("ETW-1")]          // zu kurz
    [InlineData("ETW-12345")]      // 5 Ziffern — Minimum ist 6
    [InlineData("etw-000001")]     // lowercase
    [InlineData("000001")]         // fehlendes Präfix
    [InlineData("ETW_000001")]     // Unterstrich statt Bindestrich
    public void Invalid_etw_id_format_is_rejected(string etwId)
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            PublisherEtwId = etwId
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("ETW-NNNNNN", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("ETW-000001")]
    [InlineData("ETW-123456")]
    [InlineData("ETW-9999999")]    // 7 Ziffern — erlaubt
    public void Valid_etw_id_format_passes(string etwId)
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            PublisherEtwId = etwId
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.DoesNotContain(errors, e => e.Contains("ETW", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData(AaiaLicenseModel.Paid)]
    [InlineData(AaiaLicenseModel.Subscription)]
    public void Paid_license_model_requires_price(AaiaLicenseModel model)
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            LicenseModel     = model,
            NuGetPackageId   = "AAIA.Modules.Test",
            Price            = null   // fehlt absichtlich
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("Price", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData(AaiaLicenseModel.Paid)]
    [InlineData(AaiaLicenseModel.Subscription)]
    public void Paid_license_model_requires_nuget_package_id(AaiaLicenseModel model)
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            LicenseModel   = model,
            Price          = 4.99m,
            NuGetPackageId = null   // fehlt absichtlich
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("NuGetPackageId", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Negative_price_is_rejected()
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            LicenseModel   = AaiaLicenseModel.Paid,
            NuGetPackageId = "AAIA.Modules.Test",
            Price          = -1m
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("negativ", StringComparison.OrdinalIgnoreCase));
    }

    [Theory]
    [InlineData("not-a-url")]
    [InlineData("ftp://example.com/screenshot.png")]   // nur http/https erlaubt
    [InlineData("")]
    public void Invalid_screenshot_url_is_rejected(string url)
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            Screenshots = [url]
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Contains(errors, e => e.Contains("screenshot", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Valid_marketplace_fields_produce_no_errors()
    {
        var manifest = ValidManifest(AaiaExtensionHost.AAIAS, [ServerReadPermission()]) with
        {
            PublisherEtwId = "ETW-000001",
            LicenseModel   = AaiaLicenseModel.Paid,
            NuGetPackageId = "AAIA.Modules.FritzBox",
            Price          = 4.99m,
            Currency       = "EUR",
            Repository     = "https://github.com/illeArts/aaia-fritzbox",
            IconUrl        = "https://cdn.example.com/icon.png",
            Screenshots    = ["https://cdn.example.com/screen1.png", "https://cdn.example.com/screen2.png"]
        };

        var errors = AaiaExtensionManifestRules.Validate(manifest);

        Assert.Empty(errors);
    }

    private static AaiaExtensionPermissionDto ServerReadPermission() =>
        new(
            AaiaExtensionPermissionScope.ServerRead,
            "Read server status.",
            AaiaExtensionRiskLevel.Green,
            RequiresDuki: false,
            RequiresAdminApproval: true);
}
