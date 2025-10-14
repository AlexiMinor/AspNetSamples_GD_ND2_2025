using AspNetSamples.DataAccess.Commands;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.CommandHandlers;

public class RemoveTokenCommandHandler : IRequestHandler<RemoveTokenCommand>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly UserMapper _userMapper;

    public RemoveTokenCommandHandler(GoodArticleAggregatorContext context, UserMapper userMapper)
    {
        _context = context;
        _userMapper = userMapper;
    }

    public async Task Handle(RemoveTokenCommand command, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens
            .SingleOrDefaultAsync(refreshToken => refreshToken.Id.Equals(command.Id),
                cancellationToken: cancellationToken);
        if (token != null)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

