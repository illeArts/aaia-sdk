namespace AAIA.Shared.Contracts.Marketplace;

/// <summary>Einzelner Eintrag im Marketplace-Feed.</summary>
public sealed record MarketplaceModuleDto(
    string   Id,
    string   DisplayName,
    string   Version,
    string   Description,
    string   Category,       // "Network" | "Hardware" | "Productivity" | "Security" | ...
    string   PluginClass,    // "ServerModule" | "SystemPlugin" | "ClientPlugin"
    bool     Free,
    string?  PriceHint,      // null wenn Free, sonst z.B. "€4,99/Monat" (rein dekorativ)
    string[] Platforms,
    string?  DownloadUrl,    // URL zur .zip des Paketordners (null = nur lokal installierbar)
    string?  ManifestUrl,    // URL zur aaia-extension.json
    string?  ChangelogUrl,
    string   PublisherId,
    string   MinServerVersion,
    string?  IconUrl,
    string[] Tags);

/// <summary>Der komplette Feed — wird von MarketplaceFeedService gecacht.</summary>
public sealed record MarketplaceFeedDto(
    string   Version,
    DateTimeOffset GeneratedAt,
    IReadOnlyList<MarketplaceModuleDto> Modules);

/// <summary>Anfrage für Marketplace-Install aus Feed.</summary>
public sealed record MarketplaceInstallRequest(
    string ModuleId,
    bool   OverwriteExisting = false);

/// <summary>Ergebnis eines Marketplace-Installs.</summary>
public sealed record MarketplaceInstallResult(
    bool    Success,
    string  ModuleId,
    string? Error,
    string? InstalledVersion);
