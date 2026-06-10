namespace AAIA.Shared.Contracts.Client;

public sealed record LibraryAccessDto(
    string ClientId,
    bool LibraryAvailable,
    LibraryAccessMode AccessMode,
    bool CanSearch,
    bool CanRead,
    bool CanWriteDirectly,
    bool CanSubmitWriteRequests,
    bool OfflineReadCacheAllowed,
    string? LibraryDisplayName,
    string? Explanation);
