using AspNetSamples.DataAccess.Commands;
using AspNetSamples.Database;
using AspNetSamples.Database.Entities;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace AspNetSamples.DataAccess.Handlers.CommandHandlers;

public class CreateRefreshTokenCommandHandler : IRequestHandler<CreateRefreshTokenCommand>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly IConfiguration _configuration;

    public CreateRefreshTokenCommandHandler(GoodArticleAggregatorContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task Handle(CreateRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            Id = command.Token,
            UserId = command.UserId,
            IsRevoked = false,
            ExpiryDate = DateTime.UtcNow.AddDays(Convert.ToInt32(_configuration["Jwt:RefreshTokenValidityInDays"])),
            CreationDate = DateTime.UtcNow,
            Device = command.Device
        };
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

