using AspNetSamples.Core.Dto;

namespace AspNetSamples.Services.Abstractions;

public interface IUserService
{
    public Task<UserDto> RegisterUserAsync(UserDto dto, CancellationToken token = default);
    public Task<UserDto?> TryToLoginUserAsync(string modelEmail, string modelPassword);
}