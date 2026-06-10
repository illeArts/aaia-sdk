using AAIA.Shared.Contracts.Platform;

namespace AAIA.Shared.Contracts.Auth;

public sealed record LoginRequestDto(
    string Email,
    string Password,
    string? DeviceId = null);

public sealed record LoginResponseDto(
    string AccessToken,
    string RefreshToken,
    AuthUserDto User,
    DateTimeOffset ExpiresAt);

public sealed record AuthUserDto(
    string UserId,
    string Email,
    string DisplayName,
    IReadOnlyList<string> Roles,
    IReadOnlyList<string> Permissions);

public sealed record DeviceBindRequestDto(
    DeviceInfoDto Device,
    string ClientPublicKey);

public sealed record RefreshTokenRequest(string RefreshToken);
