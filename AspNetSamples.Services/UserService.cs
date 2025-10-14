using System.Security.Cryptography;
using System.Text;
using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Commands;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AspNetSamples.Services;

public class UserService : IUserService
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public UserService(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    public async Task<UserDto> RegisterUserAsync(UserDto dto, CancellationToken token = default)
    {
        var baseRoleName = _configuration["AppSettings:BaseRoleName"];
        if (string.IsNullOrEmpty(baseRoleName))
        {
            throw new Exception("Base role name is not configured");
        }
        var baseRole = await _mediator.Send(new GetRoleByNameQuery(){RoleName = baseRoleName}, token);
        if (baseRole == null)
        {
            throw new Exception($"Role '{baseRoleName}' does not exist");
        }

        if (await IsUserWithEmailExists(dto.Email, token))
        {
            throw new ArgumentException($"User with email '{dto.Email}' already exists");
        }

        var salt = Guid.NewGuid().ToString("D");
        var hashedPassword = HashPassword(dto.Password, salt);

        dto.Id = Guid.NewGuid();
        dto.PasswordHash = Encoding.UTF8.GetString(hashedPassword);
        dto.Salt = salt;
        dto.RoleId = baseRole.Id;
        await _mediator.Send(new RegisterUserCommand() { User = dto }, token);

        dto.Role = baseRole;

        return dto;
    }

    public async Task<UserDto?> TryToLoginUserAsync(string modelEmail, string modelPassword)
    {
        var user = await _mediator.Send(new GetUserByEmailQuery() { Email = modelEmail });
        if (user == null)
        {
            return null;
        }

        var hashedPassword = Encoding.UTF8.GetString(HashPassword(modelPassword, user.Salt));
        
        return !hashedPassword.Equals(user.PasswordHash) 
            ? null 
            : user;
    }

    public async Task<UserDto?> TryToLoginUserByRefreshTokenAsync(Guid refreshToken)
    {
        var user = await _mediator.Send(new GetUserByRefreshTokenQuery() { RefreshToken = refreshToken });
        return user ?? null;
    }

    private static byte[] HashPassword(string password, string salt)
    {
        var passwordWithSalt = password + salt;
        using var sha512 = SHA512.Create();
        var hashedPassword = sha512.ComputeHash(Encoding.UTF8.GetBytes(passwordWithSalt));
        return hashedPassword;
    }

    public async Task<bool> IsUserWithEmailExists(string email, CancellationToken token = default)
    {
        var user = await _mediator.Send(new GetUserByEmailQuery() { Email = email }, token);
        return user != null;
    }

}
