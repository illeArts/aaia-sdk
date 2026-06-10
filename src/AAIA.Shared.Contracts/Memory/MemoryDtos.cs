namespace AAIA.Shared.Contracts.Memory;

public sealed record MemorySearchRequestDto(
    string Query,
    string? Type = null,
    int Limit = 20);

public sealed record MemoryDocumentDto(
    string Id,
    string Title,
    string Content,
    string Type,
    IReadOnlyDictionary<string, string>? Metadata,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

public sealed record MemorySearchResultDto(
    string Id,
    string Title,
    string Snippet,
    string Type,
    double Score);
