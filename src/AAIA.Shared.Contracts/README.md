# AAIA.Shared.Contracts

[![NuGet](https://img.shields.io/nuget/v/AAIA.Shared.Contracts)](https://www.nuget.org/packages/AAIA.Shared.Contracts)
[![Build](https://github.com/illeArts/aaia-sdk/actions/workflows/build.yml/badge.svg)](https://github.com/illeArts/aaia-sdk/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Public shared contracts, interfaces and DTOs for building AAIA modules and plugins.

## What is AAIA?

AAIA is a modular AI operating environment for local and assisted automation.  
Modules extend the AAIAS server. Plugins extend AAIA client capabilities.

## Installation

```bash
dotnet add package AAIA.Shared.Contracts
```

Or in your `.csproj`:

```xml
<PackageReference Include="AAIA.Shared.Contracts" Version="2.0.0" />
```

## What's included

| Namespace | Contents |
|---|---|
| `Aaia.Shared.Contracts.Modules` | `IAaiaModule`, `IWorkOrderSimulator`, `ModuleInfoDto` |
| `AAIA.Shared.Contracts.Extensions` | Extension manifest DTOs, permission models, validation |
| `AAIA.Shared.Contracts.Marketplace` | Marketplace feed DTOs |
| `AAIA.Shared.Contracts.Licensing` | License DTOs |
| `AAIA.Shared.Contracts.Dev` | `IDevDiagnosticsBus`, `NullDevDiagnosticsBus`, diagnostics events |
| `AAIA.Shared.Contracts.Common` | `AaiaApiResponse`, `AaiaApiError` |
| `AAIA.Shared.Contracts.Platform` | OS, architecture, device kind enums |

## Building a module

Implement `IAaiaModule`:

```csharp
using Aaia.Shared.Contracts.Modules;

public sealed class MyModule : IAaiaModule
{
    public string Id          => "my-module";
    public string DisplayName => "My Module";
    public string Version     => "1.0.0";
    public string Description => "Does something useful.";

    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<MyService>();
    }

    public void MapRoutes(WebApplication app)
    {
        app.MapGet("/api/modules/my-module/hello", () => "Hello from MyModule");
    }
}
```

Add a manifest (`aaia-extension.json`):

```json
{
  "id":          "com.example.my-module",
  "displayName": "My Module",
  "version":     "1.0.0",
  "host":        "AAIAS",
  "kind":        "Module",
  "assembly":    "MyModule.dll",
  "permissions": [],
  "supportedPlatforms": ["all"]
}
```

## Security rules

- Modules must declare all required permissions in `aaia-extension.json`
- No direct database access — all writes go through AAIAS
- No shell execution without explicit permission declaration
- No undeclared network targets

## License

MIT — see [LICENSE](LICENSE)

## Links

- [AAIA Developer Docs](https://github.com/illeArts/aaia-developer-docs) *(coming soon)*
- [Module Template](https://github.com/illeArts/aaia-module-template) *(coming soon)*
- [Extension Registry](https://github.com/illeArts/aaia-extension-registry) *(coming soon)*
