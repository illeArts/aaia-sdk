namespace AAIA.Shared.Contracts.Routes;

public sealed record ApiRouteDescriptor(
    string Method,
    string Route,
    string Group,
    string Name,
    bool IsCanonical = true,
    string? CompatibilityFor = null);
