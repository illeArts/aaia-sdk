namespace AAIA.Shared.Contracts.Marketplace;

/// <summary>
/// HTTP-Client für Lizenz-Aktivierung über die AAIA Marketplace API.
/// Wird vom AAIA Module Manager (AAIAS) aufgerufen wenn ein Nutzer
/// einen LicenseKey + E-Mail eingibt.
/// </summary>
public interface IMarketplaceLicenseClient
{
    /// <summary>
    /// Aktiviert eine WooCommerce-Lizenz und gibt das Lizenz-JWT zurück.
    ///
    /// Für Module/Plugins: LicenseJwt ist gesetzt, DownloadUrl ist null.
    /// Für LanguagePacks: DownloadUrl ist gesetzt, LicenseJwt ist null, DeviceId kann leer sein.
    ///
    /// Wirft <see cref="LicenseActivationApiException"/> wenn die API einen Fehler zurückgibt.
    /// </summary>
    Task<LicenseActivationResponse> ActivateAsync(
        LicenseActivationRequest request,
        CancellationToken        ct = default);
}
