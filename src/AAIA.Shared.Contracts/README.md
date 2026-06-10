# AAIA.Shared.Contracts

[![NuGet](https://img.shields.io/nuget/v/AAIA.Shared.Contracts)](https://www.nuget.org/packages/AAIA.Shared.Contracts)
[![Build](https://github.com/illeArts/aaia-sdk/actions/workflows/build.yml/badge.svg)](https://github.com/illeArts/aaia-sdk/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Shared contracts, interfaces and DTOs for building **AAIA modules and plugins**.  
This is the only package a module developer needs.

---

## Quickstart — 5 minutes to your first module

### 1. Create a new class library

```bash
dotnet new classlib -n AAIAS.Module.MyModule
cd AAIAS.Module.MyModule
dotnet add package AAIA.Shared.Contracts
```

### 2. Implement `IAaiaModule`

```csharp
using Aaia.Shared.Contracts.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public sealed class MyModule : IAaiaModule
{
    public string Id          => "my-module";
    public string DisplayName => "My Module";
    public string Version     => "1.0.0";
    public string Description => "Does something useful.";

    public void AddServices(IServiceCollection services)
    {
        // Register your services here
        services.AddScoped<MyService>();
    }

    public void MapRoutes(WebApplication app)
    {
        // All routes must start with /api/modules/{your-id}/
        app.MapGet("/api/modules/my-module/hello", () => "Hello from MyModule!");
    }
}
```

### 3. Add a manifest — `aaia-extension.json`

```json
{
  "id":          "com.example.my-module",
  "displayName": "My Module",
  "version":     "1.0.0",
  "host":        "AAIAS",
  "kind":        "Module",
  "assembly":    "AAIAS.Module.MyModule.dll",
  "permissions": [],
  "supportedPlatforms": ["all"]
}
```

### 4. Build and deploy

```bash
dotnet build -c Release
# Copy output to: <AAIAS data dir>/modules/my-module/
```

AAIAS discovers and loads the module automatically on startup.

---

## Naming conventions

| What | Convention | Example |
|------|-----------|---------|
| NuGet package | `AAIAS.Module.{Name}` | `AAIAS.Module.FritzBox` |
| Assembly | `AAIAS.Module.{Name}.dll` | `AAIAS.Module.FritzBox.dll` |
| Namespace | `Aaias.Modules.{Name}` | `Aaias.Modules.FritzBox` |
| Module ID | lowercase, no spaces | `fritzbox` |
| Manifest ID | reverse-domain | `com.example.fritzbox` |
| Route prefix | `/api/modules/{id}/` | `/api/modules/fritzbox/` |

---

## Available interfaces

| Interface | Namespace | Purpose |
|-----------|-----------|---------|
| `IAaiaModule` | `Aaia.Shared.Contracts.Modules` | Entry point for every module |
| `IWorkOrderSimulator` | `Aaia.Shared.Contracts.Modules` | Simulate work orders in dev mode |
| `IDevDiagnosticsBus` | `AAIA.Shared.Contracts.Dev` | Push events to the dev console |

## Key DTOs

| DTO | Namespace | Purpose |
|-----|-----------|---------|
| `AaiaApiResponse<T>` | `AAIA.Shared.Contracts.Common` | Standardized API response wrapper |
| `AaiaApiError` | `AAIA.Shared.Contracts.Common` | Error payload |
| `ModuleInfoDto` | `Aaia.Shared.Contracts.Modules` | Module metadata (serializable) |
| `AaiaExtensionManifestDto` | `AAIA.Shared.Contracts.Extensions` | Full extension manifest |

---

## Permissions

Declare special capabilities in the manifest:

```json
{
  "permissions": [
    { "scope": "network.outbound", "targets": ["192.168.1.1"] },
    { "scope": "filesystem.read",  "targets": ["/data/mymodule"] }
  ]
}
```

AAIAS rejects modules with undeclared permissions.  
Available scopes: see `AaiaExtensionPermissionScope`.

---

## Rules

- Routes must be under `/api/modules/{your-id}/` — no exceptions
- No direct database access — use the provided service interfaces
- No shell execution without declaring `shell.exec` permission
- No hardcoded network targets — declare them in the manifest

---

## Dev diagnostics

```csharp
public class MyService(IDevDiagnosticsBus bus)
{
    public void DoWork()
    {
        bus.Publish(new DevDiagnosticsEvent(
            Source:   "my-module",
            Message:  "Work done",
            Severity: DevEventSeverity.Info
        ));
    }
}
```

In unit tests: use `NullDevDiagnosticsBus.Instance` as a no-op stub.

---

## Links

- [Module Template](https://github.com/illeArts/aaia-module-template) — clone and start building
- [NuGet Package](https://www.nuget.org/packages/AAIA.Shared.Contracts)
- [Developer Docs](https://github.com/illeArts/aaia-developer-docs) *(coming soon)*
- [Extension Registry](https://github.com/illeArts/aaia-extension-registry) *(coming soon)*

## License

MIT — see [LICENSE](LICENSE)
