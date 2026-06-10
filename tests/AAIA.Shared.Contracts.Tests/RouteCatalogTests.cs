using AAIA.Shared.Contracts.Routes;
using Xunit;

namespace AAIA.Shared.Contracts.Tests;

public sealed class RouteCatalogTests
{
    [Fact]
    public void Canonical_catalog_contains_required_handshake_routes()
    {
        var routes = CanonicalApiRouteCatalog.All
            .Select(r => (r.Method, r.Route))
            .ToHashSet();

        Assert.Contains(("GET", AaiaApiRoutes.Health), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Server.Info), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Server.Status), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Server.Capabilities), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Server.Version), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Client.Config), routes);
        Assert.Contains(("POST", AaiaApiRoutes.Client.Register), routes);
        Assert.Contains(("POST", AaiaApiRoutes.Auth.Login), routes);
        Assert.Contains(("POST", AaiaApiRoutes.Auth.TokenAlias), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Auth.Me), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Extensions.Catalog), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Extensions.Installed), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Extensions.Loaded), routes);
        Assert.Contains(("POST", AaiaApiRoutes.Extensions.ValidateManifest), routes);

        Assert.Contains(("GET", AaiaApiRoutes.Memory.Search), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Core.Knowledge), routes);
        Assert.Contains(("POST", AaiaApiRoutes.Core.Knowledge), routes);
        Assert.Contains(("PUT", AaiaApiRoutes.Core.KnowledgeById), routes);
        Assert.Contains(("DELETE", AaiaApiRoutes.Core.KnowledgeById), routes);

        Assert.Contains(("GET", AaiaApiRoutes.Ollama.Status), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Ollama.Models), routes);
        Assert.Contains(("GET", AaiaApiRoutes.Ollama.Running), routes);
        Assert.Contains(("POST", AaiaApiRoutes.Ollama.Pull), routes);
        Assert.Contains(("DELETE", AaiaApiRoutes.Ollama.DeleteModel), routes);
    }

    [Fact]
    public void Canonical_catalog_has_no_duplicate_method_path_entries()
    {
        var duplicates = CanonicalApiRouteCatalog.All
            .GroupBy(r => new { r.Method, r.Route })
            .Where(g => g.Count() > 1)
            .Select(g => $"{g.Key.Method} {g.Key.Route}")
            .ToArray();

        Assert.Empty(duplicates);
    }
}
