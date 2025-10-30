using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetUserByRefreshTokenQueryHandler : IRequestHandler<GetUserByRefreshTokenQuery, UserDto?>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly UserMapper _userMapper;

    public GetUserByRefreshTokenQueryHandler(GoodArticleAggregatorContext context, UserMapper userMapper)
    {
        _context = context;
        _userMapper = userMapper;
    }

    public async Task<UserDto?> Handle(GetUserByRefreshTokenQuery request, CancellationToken cancellationToken)
    {
        var rt = await _context.RefreshTokens
            .AsNoTracking()
            .Include(us => us.User)
            .ThenInclude(us => us.Role)
            .Where(token => !token.IsRevoked)
            .SingleOrDefaultAsync(token => token.Id.Equals(request.RefreshToken), cancellationToken);

        return rt is { IsExpired: false } ? _userMapper.MapUserToUserDto(rt.User) : null;
    }
}