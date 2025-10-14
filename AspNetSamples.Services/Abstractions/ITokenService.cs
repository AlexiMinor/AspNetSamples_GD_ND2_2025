using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface ITokenService
{
    public string GenerateJwtToken(UserDto dto);

    public Task<Guid> GenerateRefreshTokenAsync(Guid userId, string? deviceName = null);
    public Task RemoveTokenAsync(Guid modelRefreshToken);
    public Task RevokeAsync(Guid modelRefreshToken);
}