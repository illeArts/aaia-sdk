namespace AAIA.Shared.Contracts.Marketplace;

/// <summary>
/// Antwort auf POST /api/marketplace/extensions/upload-signed.
/// Wird sowohl bei Erfolg als auch bei Fehler zurückgegeben —
/// HTTP-Status signalisiert Erfolg/Fehler, dieses DTO enthält Details.
/// </summary>
public sealed class SignedUploadResponse
{
    /// <summary>true = Verifikation erfolgreich, false = abgelehnt oder Fehler.</summary>
    public bool   Success      { get; init; }

    /// <summary>
    /// Maschinenlesbarer Status-String:
    /// "MarketplaceVerified", "Rejected", "AlreadyExists",
    /// "Unauthorized", "BadRequest", "ServerError"
    /// </summary>
    public string Status       { get; init; } = "";

    /// <summary>Interne Submission-ID ("sub_" + 12 hex). Null bei Fehler vor Submission-Anlage.</summary>
    public string? SubmissionId { get; init; }

    /// <summary>Menschenlesbare Statusbeschreibung.</summary>
    public string? Message      { get; init; }

    /// <summary>Hinweis auf nächsten Schritt (Phase 5.2).</summary>
    public string? NextStep     { get; init; }

    /// <summary>Marketplace-URL des Releases (Phase 5.2, im Moment null).</summary>
    public string? MarketplaceUrl { get; init; }

    /// <summary>Technisches Fehlerdetail (nur bei Reject/Error).</summary>
    public string? ErrorDetail  { get; init; }
}
