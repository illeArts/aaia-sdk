using AAIA.Shared.Contracts.Routes;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace AAIA.Shared.Contracts.Marketplace;

/// <summary>
/// HTTP-Client für den /api/marketplace/licenses/activate Endpunkt.
///
/// Registrierung in AAIAS:
/// <code>
///   services.AddHttpClient&lt;IMarketplaceLicenseClient, MarketplaceLicenseClient&gt;(c =&gt;
///       c.BaseAddress = new Uri(config[&quot;Marketplace:ApiUrl&quot;]));
/// </code>
///
/// Der Endpunkt ist anonym — kein Bearer-Token nötig.
/// Rate-Limit serverseitig: 5 req/min pro IP.
/// </summary>
public sealed class MarketplaceLicenseClient(HttpClient http) : IMarketplaceLicenseClient
{
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    /// <inheritdoc/>
    public async Task<LicenseActivationResponse> ActivateAsync(
        LicenseActivationRequest request,
        CancellationToken        ct = default)
    {
        using var resp = await http.PostAsJsonAsync(
            AaiaApiRoutes.Marketplace.ActivateLicense,
            request,
            JsonOpts,
            ct);

        if (resp.IsSuccessStatusCode)
        {
            return await resp.Content.ReadFromJsonAsync<LicenseActivationResponse>(JsonOpts, ct)
                ?? throw new LicenseActivationApiException(
                    (int)resp.StatusCode,
                    "Leere Antwort vom Marketplace-Server.");
        }

        // Fehlerdetails aus dem Body lesen
        string? errorMsg = null;
        try
        {
            var err = await resp.Content.ReadFromJsonAsync<ApiErrorBody>(JsonOpts, ct);
            errorMsg = err?.Error;
        }
        catch { /* JSON-Parse-Fehler ignorieren */ }

        throw new LicenseActivationApiException(
            (int)resp.StatusCode,
            errorMsg ?? resp.ReasonPhrase ?? "Unbekannter Fehler");
    }

    // Minimaler Error-Body gemäß Marketplace-API-Konvention
    private sealed record ApiErrorBody(string? Error);
}

/// <summary>
/// Wird geworfen wenn die Marketplace API einen Nicht-2xx-Status zurückgibt.
/// </summary>
public sealed class LicenseActivationApiException(int statusCode, string apiMessage)
    : Exception($"Lizenz-Aktivierung fehlgeschlagen (HTTP {statusCode}): {apiMessage}")
{
    /// <summary>HTTP-Statuscode der API-Antwort.</summary>
    public int StatusCode { get; } = statusCode;

    /// <summary>
    /// Gibt an ob der Fehler durch falsche Eingabe (Key, E-Mail) verursacht wurde.
    /// Bei true: Nutzer informieren, kein Retry sinnvoll.
    /// </summary>
    public bool IsUserError =>
        StatusCode is 400 or 401 or 404;

    /// <summary>Rate-Limit überschritten — kurz warten, dann erneut versuchen.</summary>
    public bool IsRateLimited => StatusCode == 429;
}
