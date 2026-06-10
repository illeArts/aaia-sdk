using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Aaia.Shared.Contracts.Modules;

/// <summary>
/// Basis-Interface für alle AAIAS-Module.
/// Jedes Modul registriert seine Services und Routen selbst.
///
/// Naming convention: AAIAS.Module.{Name}
/// Namespace:         Aaias.Modules.{Name}
/// Route-Prefix:      /api/modules/{name-lowercase}/
/// </summary>
public interface IAaiaModule
{
    string Id          { get; }   // z.B. "fritzbox"
    string DisplayName { get; }   // z.B. "FritzBox"
    string Version     { get; }   // SemVer
    string Description { get; }

    void AddServices(IServiceCollection services);
    void MapRoutes(WebApplication app);
}

/// <summary>Modul-Metadaten für Clients (serialisierbar).</summary>
public sealed record ModuleInfoDto(
    string Id,
    string DisplayName,
    string Version,
    string Description,
    bool   Enabled);
