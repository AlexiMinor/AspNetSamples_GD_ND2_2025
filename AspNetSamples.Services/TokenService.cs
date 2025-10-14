using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Commands;
using AspNetSamples.Services.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNetSamples.Services;

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMediator _mediator;

    public TokenService(ILogger<TokenService> logger, 
        IConfiguration configuration, IMediator mediator)
    {
        _logger = logger;
        _configuration = configuration;
        _mediator = mediator;
    }

    public string GenerateJwtToken(UserDto dto)
    {
        try
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                        new Claim(ClaimTypes.Email, dto.Email),
                        //new Claim(ClaimTypes.Name, dto.Name),
                        new Claim(ClaimTypes.Role, dto.Role.Name),
                        new Claim("id", dto.Id.ToString())
                    ]
                ),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:TokenValidityInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"],
                IssuedAt = DateTime.UtcNow
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        catch (Exception e)
        {
            _logger.LogError(e, "Error generating JWT token. Configuration is lost");
            throw;
        }
    }

    public async Task<Guid> GenerateRefreshTokenAsync(Guid userId, string? deviceName = null)
    {
        _logger.LogInformation($"Generating refresh token for user ID {userId}");
        var refreshToken = Guid.NewGuid();

        await _mediator.Send(new CreateRefreshTokenCommand()
        {
            UserId = userId,
            Token = refreshToken,
            Device = deviceName
        });

        return refreshToken;
    }

    public async Task RemoveTokenAsync(Guid modelRefreshToken)
    {
        await _mediator.Send(new RemoveTokenCommand()
        {
            Id = modelRefreshToken
        });
    }

    public async Task RevokeAsync(Guid modelRefreshToken)
    {
        await _mediator.Send(new RevokeTokenCommand()
        {
            Id = modelRefreshToken
        });
    }
}