using AspNetSamples.DataAccess.Commands;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.CommandHandlers;

public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly UserMapper _userMapper;

    public RevokeTokenCommandHandler(GoodArticleAggregatorContext context, UserMapper userMapper)
    {
        _context = context;
        _userMapper = userMapper;
    }

    public async Task Handle(RevokeTokenCommand command, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens
            .SingleOrDefaultAsync(refreshToken => refreshToken.Id.Equals(command.Id),
                cancellationToken: cancellationToken);
        if (token != null)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

