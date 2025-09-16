using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetUserByEmailAndPasswordHashQueryHandler : IRequestHandler<GetUserByEmailAndPasswordHashQuery, UserDto?>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly UserMapper _userMapper;

    public GetUserByEmailAndPasswordHashQueryHandler(GoodArticleAggregatorContext context, UserMapper userMapper)
    {
        _context = context;
        _userMapper = userMapper;
    }

    public async Task<UserDto?> Handle(GetUserByEmailAndPasswordHashQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.Email.Equals(request.Email) && user.PasswordHash.Equals(request.PasswordHash),
                cancellationToken);

        return _userMapper.MapUserToUserDto(user);
    }
}